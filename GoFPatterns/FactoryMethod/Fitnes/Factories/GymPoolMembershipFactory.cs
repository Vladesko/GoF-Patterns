using GoFPatterns.FactoryMethod.Fitnes.Memberships;

namespace GoFPatterns.FactoryMethod.Fitnes.Factories
{
    internal class GymPoolMembershipFactory : MembershipFactory
    {
        public override IMembership CreateMembership()
        {
            return new GymPoolMembership();
        }
    }
}
