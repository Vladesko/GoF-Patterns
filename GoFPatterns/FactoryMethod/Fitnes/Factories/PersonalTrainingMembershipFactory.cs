using GoFPatterns.FactoryMethod.Fitnes.Memberships;

namespace GoFPatterns.FactoryMethod.Fitnes.Factories
{
    internal class PersonalTrainingMembershipFactory : MembershipFactory
    {
        public override IMembership CreateMembership()
        {
            return new PersonalTrainingMembership();
        }
    }
}
