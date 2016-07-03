namespace Scrumfish.OData.Objects
{
    public abstract class Stringifier
    {
        protected object Item { get; set; }

        Stringifier(object item)
        {
            Item = item;
        }

        public abstract string Stringify();
    }
}
