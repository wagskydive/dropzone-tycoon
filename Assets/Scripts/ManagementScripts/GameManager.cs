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
        public Bank bank = new Bank();

        public CharacterHolder Characters = new CharacterHolder();
    }
}
