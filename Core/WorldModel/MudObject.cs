using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RMUD
{
    public static class RegisterBaseProperties
    {
        public static void AtStartup(RuleEngine GlobalRules)
        {
            PropertyManifest.RegisterProperty("short", typeof(String), "unnamed object", new StringSerializer());
            PropertyManifest.RegisterProperty("long", typeof(String), "", new StringSerializer());
            PropertyManifest.RegisterProperty("article", typeof(String), "a", new StringSerializer());
            PropertyManifest.RegisterProperty("nouns", typeof(NounList), null, new DefaultSerializer());
        }
    }

    public class PropertyPayload<T>
    {
        public String Name;
        public T Value;
    }

	public partial class MudObject
    {
        /// Fundamental properties of every mud object: Don't mess with them.
        public ObjectState State = ObjectState.Unitialized; 
		public String Path { get; set; }
		public String Instance { get; set; }
        public bool IsNamedObject { get { return Path != null; } }
        public bool IsAnonymousObject { get { return Path == null; } }
        public bool IsInstance { get { return IsNamedObject && Instance != null; } }
        public String GetFullName() { return Path + "@" + Instance; }
        public bool IsPersistent { get; set; }
        public MaybeNull<MudObject> Location { get; set; }

        public virtual void Initialize() { }

        public override string ToString()
        {
            if (String.IsNullOrEmpty(Path)) return this.GetType().Name;
            else return Path;
        }

        #region Properties

        // Every MudObject has a set of generic properties. Modules use these properties to store values on MudObjects.
                
        public Dictionary<String, Object> Properties = new Dictionary<string, Object>();
                

        public void SetProperty(String Name, Object Value)
        {
            if (PropertyManifest.CheckPropertyType(Name, Value))
                Properties.Upsert(Name, Value);
            else
                throw new InvalidOperationException("Setting property with object of wrong type.");
        }
        
        public T GetProperty<T>(String Name)
        {
            T r;
            if (Properties.ContainsKey(Name))
                r = (T)Properties[Name];
            else
            {
                var info = PropertyManifest.GetPropertyInformation(Name);
                if (info == null)
                    throw new InvalidOperationException("Property " + Name + " does not exist.");
                r = (T)info.DefaultValue;
            }

            var payload = new PropertyPayload<T>
            {
                Name = Name,
                Value = r
            };

            ConsiderPerformRule("modify property", this, payload);

            return payload.Value;
        }
        
        public bool HasProperty(String Name)
        {
            return Properties.ContainsKey(Name);
        }

        public RuleBuilder<MudObject, PropertyPayload<T>, PerformResult> PropertyModifier<T>(String Name)
        {
            return Perform<MudObject, PropertyPayload<T>>("modify property")
                .When((@object, payload) => payload.Name == Name)
                .Name("A generated rule to modify the property [" + Name + "]");
        }

        #endregion

		public MudObject()
		{
		    State = ObjectState.Alive;
            SetProperty("nouns", new NounList());
            IsPersistent = false;
		}

        public MudObject(String Short, String Long) : this()
        {
            SetProperty("short", Short);
            SetProperty("long", Long);

            AddNoun(Short.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            var firstChar = Short.ToLower()[0];
            if (firstChar == 'a' || firstChar == 'e' || firstChar == 'i' || firstChar == 'o' || firstChar == 'u')
                SetProperty("article", "an");
        }

        public void SimpleName(String Short, params String[] Synonyms)
        {
            SetProperty("short", Short);
            AddNoun(Short.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            AddNoun(Synonyms);
        }

        /// <summary>
        /// Destroy this object. Optionally, destroy it's children. If destroying children, destroy the
        /// children's children, etc. The most important aspect of this function is that when destroyed,
        /// persistent objects are forgotten. Destroying non-persistent objects is not necessary.
        /// </summary>
        /// <param name="DestroyChildren"></param>
        public void Destroy(bool DestroyChildren)
        {
            State = ObjectState.Destroyed;
            MudObject.ForgetInstance(this);

            if (DestroyChildren)
                foreach (var child in EnumerateObjects())
                    if (child.State != ObjectState.Destroyed)
                        child.Destroy(true);
        }
    }
}
