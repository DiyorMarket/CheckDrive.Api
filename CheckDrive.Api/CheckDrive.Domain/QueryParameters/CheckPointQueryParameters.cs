using CheckDrive.Domain.Common;
using CheckDrive.Domain.Enums;

namespace CheckDrive.Domain.QueryParameters;

public class CheckPointQueryParameters : QueryParametersBase
{
    public int? DriverId { get; set; }
    public CheckPointStatus? Status { get; set; }
    public CheckPointStage? Stage { get; set; }
    public DateFilter? DateFilter { get; set; }
}
