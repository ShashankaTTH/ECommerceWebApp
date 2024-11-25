namespace EComerceWebApp.Model
{
    public class Item
    {
        public int ITEM_ID { get; set; }
        public string NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string PRICE { get; set; }
        //public byte[] Image { get; set; }  // For storing image data as a byte array

        public string IMAGE_URL { get; set; }
    }

}
