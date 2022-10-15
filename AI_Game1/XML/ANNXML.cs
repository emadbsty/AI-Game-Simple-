using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Game1

{
    class ANNXML
    {
        static char c = (char)34;
        static char enter = (char)10;
        
        public string BuildANN(ANN ann)
        {

            string ann_str = "<root>" + "\n";

            ann_str += "<input>" + +ann.InputsX.Length + "</input> \n";
            ann_str += "<hidden>" + +ann.HN.Count + "</hidden> \n";
            ann_str += "<output>" + +ann.ON.Length + "</output> \n";
            ann_str += "<HActivation>" + ann.haf.ToString() + "</HActivation> \n";
            ann_str += "<OutActivation>" + ann.oaf.ToString() + "</OutActivation> \n";

            for (int i = 0; i < ann.HN.Count; i++)
            {
                ann_str += "<HLayer" + i + ">" + "\n" + "<NN>" + ann.HN[i].Length + "</NN> \n";
                for (int j = 0; j < ann.HN[i].Length; j++)
                {
                    ann_str += "<HN" + i + "" + j + " bias = '" + ann.HN[i][j].bias + "'>" + "\n";
                    for (int k = 0; k < ann.HN[i][j].w.Length; k++)
                    {
                        ann_str += "<w" + i + "" + j + k + ">" + ann.HN[i][j].w[k] + "" + "</w" + i + "" + j + k + ">" + "\n";
                    }
                    ann_str += "</HN" + i + "" + j + ">" + "\n";
                }
                ann_str += "</HLayer" + i + ">" + "\n" + enter;
            }
            ann_str += "<OLayer>" + "\n" + "<NN>" + ann.ON.Length + "</NN> \n";
            for (int i = 0; i < ann.ON.Length; i++)
            {
                ann_str += "<ON" + i + " bias = '" + ann.ON[i].bias + "'>" + "\n";
                for (int j = 0; j < ann.ON[i].w.Length; j++)
                {
                    ann_str += "<w" + i + "" + j + ">" + ann.ON[i].w[j] + "" + "</w" + i + "" + j + ">" + "\n";
                }
                ann_str += "</ON" + i + ">" + "\n";
            }
            ann_str += "</OLayer>" + "\n" + enter;
            ann_str += "</root>" + "\n";
            return ann_str;
        }
    }
}
