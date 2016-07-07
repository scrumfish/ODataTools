namespace Scrumfish.OData.Objects
{
    public abstract class Stringifier
    {
        protected object Item { get; set; }

        protected Stringifier(object item)
        {
            Item = item;
        }

        public abstract string Stringify();
    }
}
