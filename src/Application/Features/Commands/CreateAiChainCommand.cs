using MediatR;

public sealed record CreateAiChainCommand(
    string TetikleyiciMetin,
    string Type,
    string Domain,
    string Target,
    string Operation,
    string CalisacakKod,
    double? Confidence
) : IRequest<bool>;