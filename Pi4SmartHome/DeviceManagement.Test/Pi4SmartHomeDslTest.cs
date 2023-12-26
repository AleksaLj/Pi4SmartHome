
using Pi4SmartHomeDSL.DSL.Common.Core;
using Pi4SmartHomeDSL.DSL.Common.Interfaces;
using Pi4SmartHomeDSL.DSL.Scanner;

namespace DeviceManagement.Test
{
    public class Pi4SmartHomeDslTest
    {
        private const string _programCode =
            @"
                SEND_DEVICE_MESSAGE
                To: `test-device-1`
                MessageBody: `Hello test-device-1!`
                MessageProperties:
                BEGIN
	                Prop1 = `text`,
	                Prop2 = `text`
                END
            ";

        public static async Task Pi4SmartHomeDslExampleTest(IPi4SmartHomeDslParser? pi4SmartHomeDslParser)
        {
            var scanner = new Pi4SmartHomeDslScanner();
            await scanner.Configure(_programCode);

            var tree = await pi4SmartHomeDslParser.Parse(scanner);
        }
    }
}
