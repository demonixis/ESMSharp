using ESMSharp.Core;

namespace ESMSharp.TES4.Records
{
	public class IDLERecord : Record
	{
		protected override void ExtractSubRecords(BetterReader reader, GameID gameID, uint size)
		{
			 reader.ReadBytes((int)size);
		}
	}
}