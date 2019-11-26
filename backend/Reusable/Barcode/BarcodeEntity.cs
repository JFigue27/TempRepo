namespace Reusable.Barcode
{
    public partial class Barcode : BaseEntity
    {
        public int BarcodeKey { get; set; }

        public override int id { get { return BarcodeKey; } set { BarcodeKey = value; } }

        public string Label { get; set; }
        public string BarcodeImage { get; set; }
        public string BarcodeValue { get; set; }
    }
}