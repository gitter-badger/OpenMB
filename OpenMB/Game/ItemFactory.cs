﻿using Mogre;
using Mogre.PhysX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenMB.Mods.XML;
using OpenMB.Mods;

namespace OpenMB.Game
{
    public class ItemFactory
    {
        protected Camera cam;
        protected Scene physicsScene;
        protected ModData mod;
        protected static ItemFactory instance;
        public static ItemFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemFactory();
                }
                return instance;
            }
        }

        public void Initization(Camera cam, Scene physicsScene, ModData mod)
        {
            this.cam = cam;
            this.physicsScene = physicsScene;
            this.mod = mod;
        }

        public Item Produce(
            int id,
            string desc, string meshName, ItemValidType type,
            ItemUseAttachOption itemUseAttachOption,
            ItemHaveAttachOption itemHaveAttachOption,
            double damage, int range,
            GameWorld world, int ammoCapcity = -1,
            double amourNum = -1)
        {
            Item item = null;
            
            return item;
        }

        public Item PreProduce(ModData modData, GameWorld world, ModItemDfnXML findedItem)
        {
            var itemType = modData.ItemTypes.Where(o => o.Name == findedItem.Type).FirstOrDefault();
            if (itemType == null)
            {
                throw new Exception("Couldn't find properly item type!");
            }

            itemType.ModData = modData;

            Item item = new Item(world, itemType, findedItem);
            return item;
        }

        public Item Produce(Mods.XML.ModItemDfnXML itemXml, GameWorld world)
        {
            return null;
        }
    }
}
