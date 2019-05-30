﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenMB.Game
{
    public class ItemWeaponFactory : ItemFactory
    {
        protected static new ItemWeaponFactory instance;
        public static new ItemWeaponFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemWeaponFactory();
                }
                return instance;
            }
        }

        public Item Produce(
            int id,
            string desc, string meshName, ItemType type, 
            double damage, int range, GameWorld world)
        {
            Item item = null;
            switch(type)
            {
                case ItemType.IT_BOW:
                    item = new Bow(id, desc, meshName, world);
                    break;
                case ItemType.IT_CROSSBOW:
                    item = new Crossbow(id, desc, meshName, world);
                    break;
                case ItemType.IT_ONE_HAND_WEAPON:
                    item = new OneHandWeapon(id, desc, meshName, world);
                    break;
                case ItemType.IT_TWO_HAND_WEAPON:
                    break;
                case ItemType.IT_POLEARM:
                    break;
                case ItemType.IT_RIFLE:
                    item = new Rifle(id, desc, meshName, world);
                    break;
                case ItemType.IT_PISTOL:
                    item = new Pistol(id, desc, meshName, world);
                    break;
                case ItemType.IT_SUBMACHINE_GUN:
                    break;
                case ItemType.IT_LAUNCHER:
                    break;
            }
            item.Damage = damage;
            item.Range = range;
            return item;
        }
    }
}