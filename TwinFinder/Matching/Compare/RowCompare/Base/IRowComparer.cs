using System.Data;

namespace TwinFinder.Matching.Compare.RowCompare.Base
{
    public interface IRowComparer
    {
        CompareDefinition CompareDefinition { get; set; }

        float Compare(DataRow row1, DataRow row2);
    }
}
