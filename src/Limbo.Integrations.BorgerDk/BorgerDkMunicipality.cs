using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Limbo.Integrations.BorgerDk.Json;
using Newtonsoft.Json;

namespace Limbo.Integrations.BorgerDk {

    /// <summary>
    /// Class representing a reference to a municipality.
    /// </summary>
    [JsonConverter(typeof(BorgerDkJsonConverter))]
    public class BorgerDkMunicipality {

        #region Contants

        public static readonly BorgerDkMunicipality NoMunicipality = new BorgerDkMunicipality(0, null, null);
        public static readonly BorgerDkMunicipality AlbertslundKommune = new BorgerDkMunicipality(165, "Albertslund");
        public static readonly BorgerDkMunicipality AlleroedKommune = new BorgerDkMunicipality(201, "Allerød");
        public static readonly BorgerDkMunicipality AssensKommune = new BorgerDkMunicipality(420, "Assens");
        public static readonly BorgerDkMunicipality BallerupKommune = new BorgerDkMunicipality(151, "Ballerup");
        public static readonly BorgerDkMunicipality BillundKommune = new BorgerDkMunicipality(530, "Billund");
        public static readonly BorgerDkMunicipality BornholmsRegionskommune = new BorgerDkMunicipality(400, "Bornholm", "Bornholms Regionskommune");
        public static readonly BorgerDkMunicipality BroendbyKommune = new BorgerDkMunicipality(153, "Brøndby");
        public static readonly BorgerDkMunicipality BroenderslevKommune = new BorgerDkMunicipality(810, "Brønderslev");
        public static readonly BorgerDkMunicipality DragoerKommune = new BorgerDkMunicipality(155, "Dragør");
        public static readonly BorgerDkMunicipality EgedalKommune = new BorgerDkMunicipality(240, "Egedal");
        public static readonly BorgerDkMunicipality EsbjergKommune = new BorgerDkMunicipality(561, "Esbjerg");
        public static readonly BorgerDkMunicipality FanoeKommune = new BorgerDkMunicipality(563, "Fanø");
        public static readonly BorgerDkMunicipality FavrskovKommune = new BorgerDkMunicipality(710, "Favrskov");
        public static readonly BorgerDkMunicipality FaxeKommune = new BorgerDkMunicipality(320, "Faxe");
        public static readonly BorgerDkMunicipality FredensborgKommune = new BorgerDkMunicipality(210, "Fredensborg");
        public static readonly BorgerDkMunicipality FredericiaKommune = new BorgerDkMunicipality(607, "Fredericia");
        public static readonly BorgerDkMunicipality FrederiksbergKommune = new BorgerDkMunicipality(147, "Frederiksberg");
        public static readonly BorgerDkMunicipality FrederikshavnKommune = new BorgerDkMunicipality(813, "Frederikshavn");
        public static readonly BorgerDkMunicipality FrederikssundKommune = new BorgerDkMunicipality(250, "Frederikssund");
        public static readonly BorgerDkMunicipality FuresoeKommune = new BorgerDkMunicipality(190, "Furesø");
        public static readonly BorgerDkMunicipality FaaborgMidtfynKommune = new BorgerDkMunicipality(430, "Faaborg-Midtfyn");
        public static readonly BorgerDkMunicipality GentofteKommune = new BorgerDkMunicipality(157, "Gentofte");
        public static readonly BorgerDkMunicipality GladsaxeKommune = new BorgerDkMunicipality(159, "Gladsaxe");
        public static readonly BorgerDkMunicipality GlostrupKommune = new BorgerDkMunicipality(161, "Glostrup");
        public static readonly BorgerDkMunicipality GreveKommune = new BorgerDkMunicipality(253, "Greve");
        public static readonly BorgerDkMunicipality GribskovKommune = new BorgerDkMunicipality(270, "Gribskov");
        public static readonly BorgerDkMunicipality GuldborgsundKommune = new BorgerDkMunicipality(376, "Guldborgsund");
        public static readonly BorgerDkMunicipality HaderslevKommune = new BorgerDkMunicipality(510, "Haderslev");
        public static readonly BorgerDkMunicipality HalsnaesKommune = new BorgerDkMunicipality(260, "Halsnæs");
        public static readonly BorgerDkMunicipality HedenstedKommune = new BorgerDkMunicipality(766, "Hedensted");
        public static readonly BorgerDkMunicipality HelsingoerKommune = new BorgerDkMunicipality(217, "Helsingør");
        public static readonly BorgerDkMunicipality HerlevKommune = new BorgerDkMunicipality(163, "Herlev");
        public static readonly BorgerDkMunicipality HerningKommune = new BorgerDkMunicipality(657, "Herning");
        public static readonly BorgerDkMunicipality HilleroedKommune = new BorgerDkMunicipality(219, "Hillerød");
        public static readonly BorgerDkMunicipality HjoerringKommune = new BorgerDkMunicipality(860, "Hjørring");
        public static readonly BorgerDkMunicipality HolbaekKommune = new BorgerDkMunicipality(316, "Holbæk");
        public static readonly BorgerDkMunicipality HolstebroKommune = new BorgerDkMunicipality(661, "Holstebro");
        public static readonly BorgerDkMunicipality HorsensKommune = new BorgerDkMunicipality(615, "Horsens");
        public static readonly BorgerDkMunicipality HvidovreKommune = new BorgerDkMunicipality(167, "Hvidovre");
        public static readonly BorgerDkMunicipality HoejeTaastrupKommune = new BorgerDkMunicipality(169, "Høje-Taastrup");
        public static readonly BorgerDkMunicipality HoersholmKommune = new BorgerDkMunicipality(223, "Hørsholm");
        public static readonly BorgerDkMunicipality IkastBrandeKommune = new BorgerDkMunicipality(756, "Ikast-Brande");
        public static readonly BorgerDkMunicipality IshoejKommune = new BorgerDkMunicipality(183, "Ishøj");
        public static readonly BorgerDkMunicipality JammerbugtKommune = new BorgerDkMunicipality(849, "Jammerbugt");
        public static readonly BorgerDkMunicipality KalundborgKommune = new BorgerDkMunicipality(326, "Kalundborg");
        public static readonly BorgerDkMunicipality KertemindeKommune = new BorgerDkMunicipality(440, "Kerteminde");
        public static readonly BorgerDkMunicipality KoldingKommune = new BorgerDkMunicipality(621, "Kolding");
        public static readonly BorgerDkMunicipality KoebenhavnsKommune = new BorgerDkMunicipality(101, "København", "Københavns Kommune");
        public static readonly BorgerDkMunicipality KoegeKommune = new BorgerDkMunicipality(259, "Køge");
        public static readonly BorgerDkMunicipality LangelandKommune = new BorgerDkMunicipality(482, "Langeland");
        public static readonly BorgerDkMunicipality LejreKommune = new BorgerDkMunicipality(350, "Lejre");
        public static readonly BorgerDkMunicipality LemvigKommune = new BorgerDkMunicipality(665, "Lemvig");
        public static readonly BorgerDkMunicipality LollandKommune = new BorgerDkMunicipality(360, "Lolland");
        public static readonly BorgerDkMunicipality LyngbyTaarbaekKommune = new BorgerDkMunicipality(173, "Lyngby-Taarbæk");
        public static readonly BorgerDkMunicipality LaesoeKommune = new BorgerDkMunicipality(825, "Læsø");
        public static readonly BorgerDkMunicipality MariagerfjordKommune = new BorgerDkMunicipality(846, "Mariagerfjord");
        public static readonly BorgerDkMunicipality MiddelfartKommune = new BorgerDkMunicipality(410, "Middelfart");
        public static readonly BorgerDkMunicipality MorsoeKommune = new BorgerDkMunicipality(773, "Morsø");
        public static readonly BorgerDkMunicipality NorddjursKommune = new BorgerDkMunicipality(707, "Norddjurs");
        public static readonly BorgerDkMunicipality NordfynsKommune = new BorgerDkMunicipality(480, "Nordfyn", "Nordfyns Kommune");
        public static readonly BorgerDkMunicipality NyborgKommune = new BorgerDkMunicipality(450, "Nyborg");
        public static readonly BorgerDkMunicipality NaestvedKommune = new BorgerDkMunicipality(370, "Næstved");
        public static readonly BorgerDkMunicipality OdderKommune = new BorgerDkMunicipality(727, "Odder");
        public static readonly BorgerDkMunicipality OdenseKommune = new BorgerDkMunicipality(461, "Odense");
        public static readonly BorgerDkMunicipality OdsherredKommune = new BorgerDkMunicipality(306, "Odsherred");
        public static readonly BorgerDkMunicipality RandersKommune = new BorgerDkMunicipality(730, "Randers");
        public static readonly BorgerDkMunicipality RebildKommune = new BorgerDkMunicipality(840, "Rebild");
        public static readonly BorgerDkMunicipality RingkoebingSkjernKommune = new BorgerDkMunicipality(760, "Ringkøbing-Skjern");
        public static readonly BorgerDkMunicipality RingstedKommune = new BorgerDkMunicipality(329, "Ringsted");
        public static readonly BorgerDkMunicipality RoskildeKommune = new BorgerDkMunicipality(265, "Roskilde");
        public static readonly BorgerDkMunicipality RudersdalKommune = new BorgerDkMunicipality(230, "Rudersdal");
        public static readonly BorgerDkMunicipality RoedovreKommune = new BorgerDkMunicipality(175, "Rødovre");
        public static readonly BorgerDkMunicipality SamsoeKommune = new BorgerDkMunicipality(741, "Samsø");
        public static readonly BorgerDkMunicipality SilkeborgKommune = new BorgerDkMunicipality(740, "Silkeborg");
        public static readonly BorgerDkMunicipality SkanderborgKommune = new BorgerDkMunicipality(746, "Skanderborg");
        public static readonly BorgerDkMunicipality SkiveKommune = new BorgerDkMunicipality(779, "Skive");
        public static readonly BorgerDkMunicipality SlagelseKommune = new BorgerDkMunicipality(330, "Slagelse");
        public static readonly BorgerDkMunicipality SolroedKommune = new BorgerDkMunicipality(269, "Solrød");
        public static readonly BorgerDkMunicipality SoroeKommune = new BorgerDkMunicipality(340, "Sorø");
        public static readonly BorgerDkMunicipality StevnsKommune = new BorgerDkMunicipality(336, "Stevns");
        public static readonly BorgerDkMunicipality StruerKommune = new BorgerDkMunicipality(671, "Struer");
        public static readonly BorgerDkMunicipality SvendborgKommune = new BorgerDkMunicipality(479, "Svendborg");
        public static readonly BorgerDkMunicipality SyddjursKommune = new BorgerDkMunicipality(706, "Syddjurs");
        public static readonly BorgerDkMunicipality SoenderborgKommune = new BorgerDkMunicipality(540, "Sønderborg");
        public static readonly BorgerDkMunicipality ThistedKommune = new BorgerDkMunicipality(787, "Thisted");
        public static readonly BorgerDkMunicipality ToenderKommune = new BorgerDkMunicipality(550, "Tønder");
        public static readonly BorgerDkMunicipality TaarnbyKommune = new BorgerDkMunicipality(185, "Tårnby");
        public static readonly BorgerDkMunicipality VallensbaekKommune = new BorgerDkMunicipality(187, "Vallensbæk");
        public static readonly BorgerDkMunicipality VardeKommune = new BorgerDkMunicipality(573, "Varde");
        public static readonly BorgerDkMunicipality VejenKommune = new BorgerDkMunicipality(575, "Vejen");
        public static readonly BorgerDkMunicipality VejleKommune = new BorgerDkMunicipality(630, "Vejle");
        public static readonly BorgerDkMunicipality VesthimmerlandsKommune = new BorgerDkMunicipality(820, "Vesthimmerland", "Vesthimmerlands Kommune");
        public static readonly BorgerDkMunicipality ViborgKommune = new BorgerDkMunicipality(791, "Viborg");
        public static readonly BorgerDkMunicipality VordingborgKommune = new BorgerDkMunicipality(390, "Vordingborg");
        public static readonly BorgerDkMunicipality AeroeKommune = new BorgerDkMunicipality(492, "Ærø");
        public static readonly BorgerDkMunicipality AabenraaKommune = new BorgerDkMunicipality(580, "Aabenraa");
        public static readonly BorgerDkMunicipality AalborgKommune = new BorgerDkMunicipality(851, "Aalborg");
        public static readonly BorgerDkMunicipality AarhusKommune = new BorgerDkMunicipality(751, "Aarhus");

        #endregion

        #region Properties

        /// <summary>
        /// Gets the code/ID of the municipality.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Gets the name of the municipality.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the full name of the municipality.
        /// </summary>
        public string NameLong { get; private set; }

        /// <summary>
        /// Gets an array of all municipalities.
        /// </summary>
        public static BorgerDkMunicipality[] Values {
            get {
                return new[] {
                    NoMunicipality, KoebenhavnsKommune, FrederiksbergKommune, BallerupKommune, BroendbyKommune,
                    DragoerKommune, GentofteKommune, GladsaxeKommune, GlostrupKommune, HerlevKommune, AlbertslundKommune,
                    HvidovreKommune, HoejeTaastrupKommune, LyngbyTaarbaekKommune, RoedovreKommune, IshoejKommune,
                    TaarnbyKommune, VallensbaekKommune, FuresoeKommune, AlleroedKommune, FredensborgKommune,
                    HelsingoerKommune, HilleroedKommune, HoersholmKommune, RudersdalKommune, EgedalKommune,
                    FrederikssundKommune, GreveKommune, KoegeKommune, HalsnaesKommune, RoskildeKommune,
                    SolroedKommune, GribskovKommune, OdsherredKommune, HolbaekKommune, FaxeKommune,
                    KalundborgKommune, RingstedKommune, SlagelseKommune, StevnsKommune, SoroeKommune, LejreKommune,
                    LollandKommune, NaestvedKommune, GuldborgsundKommune, VordingborgKommune, BornholmsRegionskommune,
                    MiddelfartKommune, AssensKommune, FaaborgMidtfynKommune, KertemindeKommune, NyborgKommune,
                    OdenseKommune, SvendborgKommune, NordfynsKommune, LangelandKommune, AeroeKommune,
                    HaderslevKommune, BillundKommune, SoenderborgKommune, ToenderKommune, EsbjergKommune,
                    FanoeKommune, VardeKommune, VejenKommune, AabenraaKommune, FredericiaKommune, HorsensKommune,
                    KoldingKommune, VejleKommune, HerningKommune, HolstebroKommune, LemvigKommune, StruerKommune,
                    SyddjursKommune, NorddjursKommune, FavrskovKommune, OdderKommune, RandersKommune,
                    SilkeborgKommune, SamsoeKommune, SkanderborgKommune, AarhusKommune, IkastBrandeKommune,
                    RingkoebingSkjernKommune, HedenstedKommune, MorsoeKommune, SkiveKommune, ThistedKommune,
                    ViborgKommune, BroenderslevKommune, FrederikshavnKommune, VesthimmerlandsKommune, LaesoeKommune,
                    RebildKommune, MariagerfjordKommune, JammerbugtKommune, AalborgKommune, HjoerringKommune
                };
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance from the specified <paramref name="code"/> and <paramref name="name"/>.
        /// </summary>
        /// <param name="code">The code/ID of the municipality.</param>
        /// <param name="name">The name of the municipality.</param>
        internal BorgerDkMunicipality(int code, string name) {
            Code = code;
            Name = name;
            NameLong = name + " Kommune";
        }

        /// <summary>
        /// Initializes a new instance from the specified <paramref name="code"/>, <paramref name="name"/> and
        /// <paramref name="nameLong"/>.
        /// </summary>
        /// <param name="code">The code/ID of the municipality.</param>
        /// <param name="name">The name of the municipality.</param>
        /// <param name="nameLong">The full name of the municipality.</param>
        internal BorgerDkMunicipality(int code, string name, string nameLong) {
            Code = code;
            Name = name;
            NameLong = nameLong;
        }

        #endregion

        #region Static methods

        public static IEnumerable<BorgerDkMunicipality> Where(Func<BorgerDkMunicipality, bool> predicate) {
            return Values.Where(predicate);
        }

        public static BorgerDkMunicipality FirstOrDefault(Func<BorgerDkMunicipality, bool> predicate) {
            return Values.FirstOrDefault(predicate) ?? NoMunicipality;
        }

        public static BorgerDkMunicipality GetFromCode(int code) {
            return Values.FirstOrDefault(x => x.Code == code) ?? NoMunicipality;
        }

        public static BorgerDkMunicipality GetFromCode(string code) {
            return Values.FirstOrDefault(x => x.Code.ToString(CultureInfo.InvariantCulture) == code) ?? NoMunicipality;
        }

        public static bool TryGetFromCode(int code, out BorgerDkMunicipality municipality) {
            municipality = Values.FirstOrDefault(x => x.Code == code);
            return municipality != null;
        }

        public static bool TryGetFromCode(string code, out BorgerDkMunicipality municipality) {
            municipality = Values.FirstOrDefault(x => x.Code.ToString(CultureInfo.InvariantCulture) == code);
            return municipality != null;
        }

        #endregion

    }

}