
using AdminManagementDSL.AdminDSL.Common.Core;
using AdminManagementDSL.AdminDSL.Common.Interfaces;
using AdminManagementDSL.AdminDSL.Scanner;

namespace AdminManagementDSL.Test
{
    internal class AdminDSLTest
    {
        internal async Task TestAdminDSLExample(IAdminDSLParser? parser, IAdminDSLInterpreter? interpreter)
        {
            string adminDSL = @"
                                    PI4SMARTHOMEADMIN.PROVISION

                                    BEGIN
                                    {
	                                    DEFINE ESTATE: TABLE
	                                    EstateType: FIELD = `Home` AND Name: FIELD = `testName` AND Addr: FIELD = `testAddr` AND Description: FIELD = `testDesc`;

	                                    DEFINE ESTATE_USERS: TABLE
	                                    Users: AGGR = `user1@gmail.com, user2@gmail.com, user3@gmail.com`;

	                                    DEFINE ESTATE_PARTS: TABLE
	                                    EstateParts: AGGR = `Part1, Part2, Part3`;

	                                    DEFINE ESTATE_DEVICES: TABLE
	                                    DeviceType: FIELD = `devType1` AND IsActive: FIELD = `true` AND EstatePart: FIELD = `estatePart1`;
                                    }
                                    END
                               ";

            if (parser == null)
                return;
            if (interpreter == null)
                return;

            IAdminDSLScanner scanner = new AdminDSLScanner();
            await scanner.Configure(adminDSL);

            /*Token? token = null;
            do
            {
                token = await scanner.GetNextToken();

            } while (token.TokenType != TokenTypeEnum.EOF);*/

            var tree = await parser.Parse(scanner);
            var sqlTables = await interpreter.Interpret(tree);

            var result = 1;
        }
    }
}
