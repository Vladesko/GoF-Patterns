using GoFPatterns.FactoryMethod.Fitnes.Memberships;

namespace GoFPatterns.FactoryMethod.Fitnes.Factories
{
    internal class GymMembershipFactory : MembershipFactory
    {
        public override IMembership CreateMembership()
        {
            return new GymMembership();
        }
    }
}
