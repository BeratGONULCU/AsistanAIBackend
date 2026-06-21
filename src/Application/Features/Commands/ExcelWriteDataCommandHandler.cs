using GeminiAsistanBackend.Application.DTOs;
using GeminiAsistanBackend.Application.Interfaces;
using GeminiAsistanBackend.Domain.Entities;
using GeminiAsistanBackend.Domain.Enums;
using MediatR;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GeminiAsistanBackend.Application.Features.Commands;

public sealed class ExcelWriteDataCommandHandler : IRequestHandler<ExcelWriteDataCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public ExcelWriteDataCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ExcelWriteDataCommand request, CancellationToken cancellationToken)
    {
        if (request.FileStream == null || request.FileStream.Length == 0 )
            throw new InvalidOperationException("Geçerli bir Excel dosyası yüklenmedi.");

        var extension = Path.GetExtension(request.FileName)?.ToLowerInvariant();
        if(extension != ".xlsx" && extension != ".xls")
        {
            throw new InvalidOperationException("sadece excel dosyası eklenmesi gerekiyor.");
        }

        //long fileLengthCheck = new FileInfo(request.FileName).Length;
        long fileLengthCheck1 = request.FileStream.Length;

        if (fileLengthCheck1 > 20 * 1024) { 
            throw new InvalidOperationException("dosya boyutu 20kb dan büyük olamaz."); 
        }

        if (request.FileStream.CanSeek) request.FileStream.Position = 0;

        // 1. Excel verilerini belleğe çekmek için geçici listeler oluşturuyoruz
        var rows = new List<ExcelImportResponse>();

        // 2. Excel dosyasını Stream olarak açıp MiniExcel ile hızlıca okuyoruz
        using (var stream = request.FileStream)
        {
            rows = stream.Query<ExcelImportResponse>().ToList();
        }

        if (rows.Count == 0) return false;

        // 3. Toplu veri girişinde güvenlik için tek bir Transaction başlatıyoruz
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Performans için listeleri döngüden önce hazırlıyoruz
            foreach (var row in rows)
            {
                // Validasyonlar (Gerekirse)
                if (string.IsNullOrWhiteSpace(row.TetikleyiciMetin) || string.IsNullOrWhiteSpace(row.Type))
                    continue; // Hatalı boş satır varsa atla

                // A. Ses Tetikleyicisi Oluştur
                var ses = new SesTetikleyicisi
                {
                    TetikleyiciMetin = row.TetikleyiciMetin.Trim(),
                    EklenmeTuru = EklenmeTuru.AI_LEARNED,
                    llm_confidence_score = row.Confidence
                };
                await _unitOfWork.SesTetikleyicileri.AddAsync(ses, cancellationToken);

                // B. Cihaz Komutu Oluştur
                var komut = new CihazKomutu
                {
                    type = row.Type.Trim(),
                    domain = row.Domain?.Trim(),
                    target = row.Target?.Trim(),
                    operation = row.Operation.Trim(),
                    CalisacakKod = row.CalisacakKod?.Trim() ?? string.Empty,
                    Aciklama = "{}" 
                };
                await _unitOfWork.CihazKomutlari.AddAsync(komut, cancellationToken);

                // C. İLİŞKİYİ KURMA (Çok Önemli)
                // EF Core'un Navigation Property özelliğini kullanıyoruz. 
                // SaveChanges çağrılmadan önce ID'ler 0 olsa bile, EF Core bellekte bu iki nesneyi 
                // birbirine bağlayacağımızı anlar ve ID'leri veritabanı düzeyinde otomatik eşleştirir!
                var relation = new TetikleyiciKomut
                {
                    Tetikleyici = ses, // Manuel ID vermek yerine doğrudan nesneleri bağlıyoruz
                    Komut = komut
                };
                await _unitOfWork.TetikleyiciKomutlar.AddAsync(relation, cancellationToken);
            }

            // 4. Döngü bitti! 200 satırlık verinin tamamını TEK BİR SQL SORGUSUYLA diske yazıyoruz.
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Her şey sorunsuz bittiyse transaction onaylanır
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        catch (Exception)
        {
            // 200 satırdan 1 tanesi bile patlasa veritabanını korumak için hepsini geri al!
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}