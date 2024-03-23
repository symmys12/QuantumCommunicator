using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace QuantumCommunicator.Items{
    public class StrangeMoonShardDropCondition : IItemDropRuleCondition
    {
        public bool CanDrop(DropAttemptInfo info)
        {
            return true;
        }
        public bool CanShowItemDropInUI()
        {
            throw new System.NotImplementedException();
        }

        public string GetConditionDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}