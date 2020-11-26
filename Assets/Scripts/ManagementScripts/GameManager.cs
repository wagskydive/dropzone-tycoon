using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FinanceLogic;
using CharacterLogic;

namespace ManagementScripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private string[] StatTypes;


        public Bank bank;

        public CharacterDataHolder Characters;


        private void Awake()
        {
            bank = new Bank();
            Characters = new CharacterDataHolder(StatTypes);

        }

    }
}
