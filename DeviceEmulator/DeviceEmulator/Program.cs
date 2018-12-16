using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DeviceEmulator
{
	class Program
	{
		static void Main(string[] args)
		{
			const String noStationId = "==not defined==";
			String[] parts = new string[] { "help" };
			String stationId = noStationId;
			String markId = String.Empty;

			do
			{
				switch (parts[0])
				{
					case "quit":
						return;

					case "help":
						Console.WriteLine("Allowed commands:");
						Console.WriteLine(" help");
						Console.WriteLine(" quit");
						Console.WriteLine(" setstationid #ID");
						Console.WriteLine(" foundmark #ID");
						Console.WriteLine(" lostmark #ID");
						break;

					case "setstationid":
						if (parts[1].Length > 0)
						{
							stationId = parts[1];
							Console.WriteLine("Station ID is set to " + stationId);
						}
						else
						{
							Console.WriteLine("Error: no station ID defined");
						}
						break;

					case "foundmark":
						if (stationId == noStationId)
						{
							Console.WriteLine("Station ID is not set yet");
						}
						else if (parts[1].Length > 0)
						{
							markId = parts[1];
							RequestServer("POST", stationId, markId);
							Console.WriteLine("Found mark #" + markId);
						}
						else
						{
							Console.WriteLine("Error: no mark ID defined");
						}
						break;

					case "lostmark":
						if (stationId == noStationId)
						{
							Console.WriteLine("Station ID is not set yet");
						}
						else if (parts[1].Length > 0)
						{
							markId = parts[1];
							RequestServer("DELETE", stationId, markId);
							Console.WriteLine("Lost mark #" + markId);
						}
						else
						{
							Console.WriteLine("Error: no mark ID defined");
						}
						break;

					default:
						Console.WriteLine("Unknown Command: " + parts[0]);
						break;
				}

				Console.Write("Next command? ");
				parts = Console.ReadLine().Split(' ');
				if(parts.Length < 2)
				{
					Array.Resize(ref parts, 2);
					parts[1] = String.Empty;
				}

			} while (true);
		}

		private static void RequestServer(string operation, string stationId, string markId)
		{
			WebRequest request = WebRequest.Create(ConfigurationManager.AppSettings[operation == "DELETE" ? "lostmarkUrl" : "foundmarkUrl"]);
			request.Method = operation;
			byte[] dataStream = Encoding.UTF8.GetBytes(String.Format("station={0}&mark={1}", stationId, markId));
            #region "http post data"
            Stream newStream = request.GetRequestStream();
			newStream.Write(dataStream, 0, dataStream.Length);
			newStream.Close();
            #endregion
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		}
	}
}
