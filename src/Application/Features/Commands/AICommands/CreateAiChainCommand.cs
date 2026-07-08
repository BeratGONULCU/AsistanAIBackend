using MediatR;

public sealed record CreateAiChainCommand( // sadece veritabanı kaydı ve get olacaksa record , iş mantığı olacaksa class (epostagonder() gibi methodlar varsa)
    string TetikleyiciMetin,
    string Type,
    string Domain,
    string Target,
    string Operation,
    string CalisacakKod,
    double? Confidence
) : IRequest<bool>; // IRequest olursa dönüş değeri yok.