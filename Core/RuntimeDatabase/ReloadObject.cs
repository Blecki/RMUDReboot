using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;

namespace RMUD
{
    public partial class RuntimeDatabase : WorldDataService
    {
        override public MudObject ReloadObject(String Path)
		{
            Path = Path.Replace('\\', '/');

			if (NamedObjects.ContainsKey(Path))
			{
				var existing = NamedObjects[Path];
				var newObject = CompileObject(Path);
				if (newObject == null)  return null;

                existing.State = ObjectState.Destroyed;
				NamedObjects.Upsert(Path, newObject);
                MudObject.InitializeObject(newObject);

				//Preserve contents
                    foreach (var item in existing.EnumerateObjectsAndRelloc())
                    {
                        newObject.Add(item.Item1, item.Item2);
                        item.Item1.Location = newObject;
                    }
                 
				//Preserve location
				if (existing is MudObject _existing && newObject is MudObject _newObject)
				{
                    if (_existing.Location.HasValue(out var loc) && _newObject.Location.HasValue(out var newLoc))
					{
                        var relloc = loc.RelativeLocationOf(existing);
						MudObject.Move(newObject as MudObject, newLoc, relloc);
						MudObject.Move(existing as MudObject, null, RelativeLocations.NONE);
					}
				}

                existing.Destroy(false);

				return newObject;
			}
			else
				return GetObject(Path);
		}
    }
}
