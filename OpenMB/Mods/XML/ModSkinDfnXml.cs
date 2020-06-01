﻿using Mogre;
using OpenMB.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OpenMB.Mods.XML
{
    [XmlRoot("Skin")]
    public class ModSkinDfnXML
    {
        [XmlElement("CharacterSkin")]
        public List<ModCharacterSkinDfnXML> CharacterSkinList { get; set; }
    }

    [XmlRoot("CharacterSkin")]
    public class ModCharacterSkinDfnXML
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlAttribute("OritentionOffset")]
        public string OritentionOffst { get; set; }
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("HasParts")]
        public bool HasParts { get; set; }
        [XmlElement("Mesh")]
        public string Mesh { get; set; }
        [XmlElement("Skeleton")]
		public string Skeleton { get; set; }
		[XmlElement("SkinParts")]
        public List<CharacterSkinPartsDfnXML> SkinParts { get; set; }
        [XmlArray("Anims")]
        [XmlArrayItem("Anim")]
        public List<ModSkinAnimationDfnXml> SkinAnimations { get; set; }
        [XmlArray("Slots")]
        [XmlArrayItem("Slot")]
        public List<ModCharacterSkinSlot> Slots { get; set; }

        public ModSkinAnimationDfnXml this[ChaAnimType chaAnimType]
		{
			get
			{
				return SkinAnimations.Where(o => o.Type == chaAnimType).FirstOrDefault();
			}
		}

        public Vector3 OritentionOffset
        {
            get
            {
                string[] tokens = OritentionOffst.Split(',');
                return new Vector3(
                    float.Parse(tokens[0]),
                    float.Parse(tokens[1]),
                    float.Parse(tokens[2])
                );
            }
        }
    }

    [XmlRoot("SkinParts")]
    public class CharacterSkinPartsDfnXML
    {
        [XmlElement("SkinPart")]
        public List<CharacterSkinPartDfnXML> SkinParts { get; set; }
    }

    public enum CharacterSkinPartType
    {
        HEAD,
        HALF_HEAD,
        BODY,
        LEFT_HAND,
        RIGHT_HAND,
        LEFT_CALF,
        RIGHT_CALF
    }

    public class CharacterSkinPartDfnXML
    {
        [XmlAttribute]
        public CharacterSkinPartType Type;
        [XmlAttribute]
        public string BindBone;
        [XmlText]
        public string Mesh;
    }

    public class ModSkinAnimationDfnXml
    {
        [XmlAttribute("Type")]
        public ChaAnimType Type { get; set; }

		[XmlText]
		public string AnimID { get; set; }
	}

	public class ModSkinSubAnimationDfnXml
	{
		public AnimPlayType Type { get; set; }

		[XmlText]
		public string Name { get; set; }
	}
}
