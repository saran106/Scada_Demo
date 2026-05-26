using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada_Demo.Models;
using Scada_Demo.Services.Interface;

namespace Scada_Demo.Services.PLC
{
    public class Dummy_PLC : IPLCService
    {
      
            private Batch_Output batchOutput =
                new Batch_Output()
                {
                    BatchMode = "CoarseFine",
                    GateFullOpen = 800,
                    GateHalfOpen = 1000
                };

            private Gate_Sequence gateSequence =
                new Gate_Sequence()
                {
                    Agg1 = 1,
                    Agg2 = 2,
                    Agg3 = 3,
                    Agg4 = 4
                };

            public void Connect()
            {

            }

            public void Disconnect()
            {

            }

            public Batch_Output ReadBatchOutput()
            {
                return batchOutput;
            }

            public void WriteBatchOutput(Batch_Output model)
            {
                batchOutput = model;
            }

            public Gate_Sequence ReadGateSequence()
            {
                return gateSequence;
            }

            public void WriteGateSequence(Gate_Sequence model)
            {
                gateSequence = model;
            }
        
    }
}
