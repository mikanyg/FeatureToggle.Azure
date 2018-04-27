namespace FeatureToggle.Azure.Providers
{
    public class BooleanFeatureToggleDocument : FeatureToggleDocument
    {
        public BooleanFeatureToggleDocument() : base() { }        

        public BooleanFeatureToggleDocument(string toggleName) : base(toggleName)
        {            
            Enabled = false;
        }
                
        public bool Enabled { get; set; }
    }
}
