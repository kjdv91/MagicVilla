using Village_API.Models.Dto;

namespace Village_API.Datos
{
    public static class VillageStore
    {
        public static List<VillageDto> villaList = new List<VillageDto> {
            new VillageDto
            {
                Id=1, Name = "Villa Bonita", Capacity = 4, SqauerMeter="125 mt2"
            },

             new VillageDto
            {
                Id=2, Name = "Villa Marquesi", Capacity = 7, SqauerMeter="250 mt2"
            }
            };
    }
}
