using Scada_Demo.Models;

namespace Scada_Demo.Services.Interface
{
    public interface IPLCService
    {
        Batch_Output ReadBatchOutput();

        void WriteBatchOutput(Batch_Output model);

        Gate_Sequence ReadGateSequence();

        void WriteGateSequence(Gate_Sequence model);
    }
}