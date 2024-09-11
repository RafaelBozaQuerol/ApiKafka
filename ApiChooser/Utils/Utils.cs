using System.Text;

namespace ApiChooser.Utils
{
    public class Utils
    {
        public string MensajeApisDisponibles(int endpoints_count)
        {
            StringBuilder sb = new();
            sb.Append($"Tenemos {endpoints_count} APIs disponibles para su consumo. Indique: ");
            for (int i = 1; i <= endpoints_count; i++)
            {
                sb.Append($"{i}");
                if (i < endpoints_count)
                {
                    sb.Append(" o ");
                }
            }
            sb.Append(" para ejecutar");
            return sb.ToString();
        }

        public void LogMessage(string Message, string FileName)
        {
            try
            {
                using (TextWriter tw = new StreamWriter(FileName, true))
                {
                    tw.Write(Message);
                }
            }
            catch (Exception ex)  
            {
                System.Diagnostics.Trace.Write(ex.ToString());
            }
        }
    }
}
