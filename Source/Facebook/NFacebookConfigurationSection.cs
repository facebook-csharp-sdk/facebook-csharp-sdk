
namespace Facebook
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Represents the Facebook configuration section.
    /// </summary>
    public class NFacebookConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("apps", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(FacebookApplicationSettingsConfigElementCollection),
            AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public FacebookApplicationSettingsConfigElementCollection Apps
        {
            get { return this["apps"] as FacebookApplicationSettingsConfigElementCollection; }
        }
    }

    public class FacebookApplicationSettingsConfigElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets or sets the default Facebook application name.
        /// </summary>
        [ConfigurationProperty("defaultAppName")]
        public string DefaultAppName
        {
            get { return (string)this["defaultAppName"]; }
            set { this["defaultAppName"] = value; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        public FacebookApplicationSettingsConfigElement this[int index]
        {
            get
            {
                return (FacebookApplicationSettingsConfigElement)BaseGet(index);
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        public void Add(FacebookApplicationSettingsConfigElement element)
        {
            BaseAdd(element);
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FacebookApplicationSettingsConfigElement)element).AppName;
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FacebookApplicationSettingsConfigElement();
        }

        public void Remove(FacebookApplicationSettingsConfigElement element)
        {
            BaseRemove(element.AppName);
        }

        public void Remove(string appName)
        {
            BaseRemove(appName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass",
        Justification = "Reviewed. Suppression is OK here.")]
    public class FacebookApplicationSettingsConfigElement : ConfigurationElement, IFacebookAppSettings
    {
        [ConfigurationProperty("appName", IsRequired = true)]
        public string AppName
        {
            get { return (string)this["appName"]; }
            set { this["appName"] = value; }
        }

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["appId"]; }
            set { this["appId"] = value; }
        }

        public string ApiKey
        {
            get { throw new System.NotImplementedException(); }
        }

        [ConfigurationProperty("appSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["appSecret"]; }
            set { this["appSecret"] = value; }
        }

        [ConfigurationProperty("siteUrl", IsRequired = false)]
        public string SiteUrl
        {
            get { return (string)this["siteUrl"]; }
            set { this["siteUrl"] = value; }
        }

        [ConfigurationProperty("canvasPage", IsRequired = false)]
        public string CanvasPage
        {
            get { return (string)this["canvasPage"]; }
            set { this["canvasPage"] = value; }
        }

        [ConfigurationProperty("canvasUrl", IsRequired = false)]
        public string CanvasUrl
        {
            get { return (string)this["canvasUrl"]; }
            set { this["canvasUrl"] = value; }
        }
    }
}