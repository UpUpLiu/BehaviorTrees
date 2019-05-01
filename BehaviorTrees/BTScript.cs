﻿// Copyright(c) 2015-2019 Eugeny Novikov. Code under MIT license.

using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace BehaviorTrees
{
	/// <summary>
	/// 
	/// </summary>
	[DataContract]
	public class BTScript
	{
		[DataMember]
		public Node		BehaviorTree { get; private set; }
		public string	Name { get; set; }
		public bool		Saved { get; set; }

		public BTScript(string name, Node behaviorTree)
		{
			BehaviorTree = behaviorTree;
			Name = name;
		}

		public void Save(string fileName)
		{
			using (StreamWriter file = File.CreateText(fileName))
			{
				JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
				{
					TypeNameHandling = TypeNameHandling.Auto,
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					Formatting = Formatting.Indented
				});

				serializer.Serialize(file, this);
			}

			Name = Path.GetFileNameWithoutExtension(fileName);
			Saved = true;
		}

		public static BTScript Load(string fileName)
		{
			using (StreamReader file = File.OpenText(fileName))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.TypeNameHandling = TypeNameHandling.Auto;
				var script = (BTScript)serializer.Deserialize(file, typeof(BTScript));
				script.Saved = true;
				script.Name = Path.GetFileNameWithoutExtension(fileName);
				return script;
			}
		}
	}
}
