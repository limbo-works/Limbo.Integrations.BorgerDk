using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Limbo.Integrations.BorgerDk.Json;
using Newtonsoft.Json;

namespace Limbo.Integrations.BorgerDk;

/// <summary>
/// Class representing a reference to a municipality.
/// </summary>
[JsonConverter(typeof(BorgerDkJsonConverter))]
public class BorgerDkMunicipality {

    #region Contants

    public static readonly BorgerDkMunicipality NoMunicipality = new(0, string.Empty, string.Empty);
    public static readonly BorgerDkMunicipality AlbertslundKommune = new(165, "Albertslund");
    public static readonly BorgerDkMunicipality AlleroedKommune = new(201, "Allerød");
    public static readonly BorgerDkMunicipality AssensKommune = new(420, "Assens");
    public static readonly BorgerDkMunicipality BallerupKommune = new(151, "Ballerup");
    public static readonly BorgerDkMunicipality BillundKommune = new(530, "Billund");
    public static readonly BorgerDkMunicipality BornholmsRegionskommune = new(400, "Bornholm", "Bornholms Regionskommune");
    public static readonly BorgerDkMunicipality BroendbyKommune = new(153, "Brøndby");
    public static readonly BorgerDkMunicipality BroenderslevKommune = new(810, "Brønderslev");
    public static readonly BorgerDkMunicipality DragoerKommune = new(155, "Dragør");
    public static readonly BorgerDkMunicipality EgedalKommune = new(240, "Egedal");
    public static readonly BorgerDkMunicipality EsbjergKommune = new(561, "Esbjerg");
    public static readonly BorgerDkMunicipality FanoeKommune = new(563, "Fanø");
    public static readonly BorgerDkMunicipality FavrskovKommune = new(710, "Favrskov");
    public static readonly BorgerDkMunicipality FaxeKommune = new(320, "Faxe");
    public static readonly BorgerDkMunicipality FredensborgKommune = new(210, "Fredensborg");
    public static readonly BorgerDkMunicipality FredericiaKommune = new(607, "Fredericia");
    public static readonly BorgerDkMunicipality FrederiksbergKommune = new(147, "Frederiksberg");
    public static readonly BorgerDkMunicipality FrederikshavnKommune = new(813, "Frederikshavn");
    public static readonly BorgerDkMunicipality FrederikssundKommune = new(250, "Frederikssund");
    public static readonly BorgerDkMunicipality FuresoeKommune = new(190, "Furesø");
    public static readonly BorgerDkMunicipality FaaborgMidtfynKommune = new(430, "Faaborg-Midtfyn");
    public static readonly BorgerDkMunicipality GentofteKommune = new(157, "Gentofte");
    public static readonly BorgerDkMunicipality GladsaxeKommune = new(159, "Gladsaxe");
    public static readonly BorgerDkMunicipality GlostrupKommune = new(161, "Glostrup");
    public static readonly BorgerDkMunicipality GreveKommune = new(253, "Greve");
    public static readonly BorgerDkMunicipality GribskovKommune = new(270, "Gribskov");
    public static readonly BorgerDkMunicipality GuldborgsundKommune = new(376, "Guldborgsund");
    public static readonly BorgerDkMunicipality HaderslevKommune = new(510, "Haderslev");
    public static readonly BorgerDkMunicipality HalsnaesKommune = new(260, "Halsnæs");
    public static readonly BorgerDkMunicipality HedenstedKommune = new(766, "Hedensted");
    public static readonly BorgerDkMunicipality HelsingoerKommune = new(217, "Helsingør");
    public static readonly BorgerDkMunicipality HerlevKommune = new(163, "Herlev");
    public static readonly BorgerDkMunicipality HerningKommune = new(657, "Herning");
    public static readonly BorgerDkMunicipality HilleroedKommune = new(219, "Hillerød");
    public static readonly BorgerDkMunicipality HjoerringKommune = new(860, "Hjørring");
    public static readonly BorgerDkMunicipality HolbaekKommune = new(316, "Holbæk");
    public static readonly BorgerDkMunicipality HolstebroKommune = new(661, "Holstebro");
    public static readonly BorgerDkMunicipality HorsensKommune = new(615, "Horsens");
    public static readonly BorgerDkMunicipality HvidovreKommune = new(167, "Hvidovre");
    public static readonly BorgerDkMunicipality HoejeTaastrupKommune = new(169, "Høje-Taastrup");
    public static readonly BorgerDkMunicipality HoersholmKommune = new(223, "Hørsholm");
    public static readonly BorgerDkMunicipality IkastBrandeKommune = new(756, "Ikast-Brande");
    public static readonly BorgerDkMunicipality IshoejKommune = new(183, "Ishøj");
    public static readonly BorgerDkMunicipality JammerbugtKommune = new(849, "Jammerbugt");
    public static readonly BorgerDkMunicipality KalundborgKommune = new(326, "Kalundborg");
    public static readonly BorgerDkMunicipality KertemindeKommune = new(440, "Kerteminde");
    public static readonly BorgerDkMunicipality KoldingKommune = new(621, "Kolding");
    public static readonly BorgerDkMunicipality KoebenhavnsKommune = new(101, "København", "Københavns Kommune");
    public static readonly BorgerDkMunicipality KoegeKommune = new(259, "Køge");
    public static readonly BorgerDkMunicipality LangelandKommune = new(482, "Langeland");
    public static readonly BorgerDkMunicipality LejreKommune = new(350, "Lejre");
    public static readonly BorgerDkMunicipality LemvigKommune = new(665, "Lemvig");
    public static readonly BorgerDkMunicipality LollandKommune = new(360, "Lolland");
    public static readonly BorgerDkMunicipality LyngbyTaarbaekKommune = new(173, "Lyngby-Taarbæk");
    public static readonly BorgerDkMunicipality LaesoeKommune = new(825, "Læsø");
    public static readonly BorgerDkMunicipality MariagerfjordKommune = new(846, "Mariagerfjord");
    public static readonly BorgerDkMunicipality MiddelfartKommune = new(410, "Middelfart");
    public static readonly BorgerDkMunicipality MorsoeKommune = new(773, "Morsø");
    public static readonly BorgerDkMunicipality NorddjursKommune = new(707, "Norddjurs");
    public static readonly BorgerDkMunicipality NordfynsKommune = new(480, "Nordfyn", "Nordfyns Kommune");
    public static readonly BorgerDkMunicipality NyborgKommune = new(450, "Nyborg");
    public static readonly BorgerDkMunicipality NaestvedKommune = new(370, "Næstved");
    public static readonly BorgerDkMunicipality OdderKommune = new(727, "Odder");
    public static readonly BorgerDkMunicipality OdenseKommune = new(461, "Odense");
    public static readonly BorgerDkMunicipality OdsherredKommune = new(306, "Odsherred");
    public static readonly BorgerDkMunicipality RandersKommune = new(730, "Randers");
    public static readonly BorgerDkMunicipality RebildKommune = new(840, "Rebild");
    public static readonly BorgerDkMunicipality RingkoebingSkjernKommune = new(760, "Ringkøbing-Skjern");
    public static readonly BorgerDkMunicipality RingstedKommune = new(329, "Ringsted");
    public static readonly BorgerDkMunicipality RoskildeKommune = new(265, "Roskilde");
    public static readonly BorgerDkMunicipality RudersdalKommune = new(230, "Rudersdal");
    public static readonly BorgerDkMunicipality RoedovreKommune = new(175, "Rødovre");
    public static readonly BorgerDkMunicipality SamsoeKommune = new(741, "Samsø");
    public static readonly BorgerDkMunicipality SilkeborgKommune = new(740, "Silkeborg");
    public static readonly BorgerDkMunicipality SkanderborgKommune = new(746, "Skanderborg");
    public static readonly BorgerDkMunicipality SkiveKommune = new(779, "Skive");
    public static readonly BorgerDkMunicipality SlagelseKommune = new(330, "Slagelse");
    public static readonly BorgerDkMunicipality SolroedKommune = new(269, "Solrød");
    public static readonly BorgerDkMunicipality SoroeKommune = new(340, "Sorø");
    public static readonly BorgerDkMunicipality StevnsKommune = new(336, "Stevns");
    public static readonly BorgerDkMunicipality StruerKommune = new(671, "Struer");
    public static readonly BorgerDkMunicipality SvendborgKommune = new(479, "Svendborg");
    public static readonly BorgerDkMunicipality SyddjursKommune = new(706, "Syddjurs");
    public static readonly BorgerDkMunicipality SoenderborgKommune = new(540, "Sønderborg");
    public static readonly BorgerDkMunicipality ThistedKommune = new(787, "Thisted");
    public static readonly BorgerDkMunicipality ToenderKommune = new(550, "Tønder");
    public static readonly BorgerDkMunicipality TaarnbyKommune = new(185, "Tårnby");
    public static readonly BorgerDkMunicipality VallensbaekKommune = new(187, "Vallensbæk");
    public static readonly BorgerDkMunicipality VardeKommune = new(573, "Varde");
    public static readonly BorgerDkMunicipality VejenKommune = new(575, "Vejen");
    public static readonly BorgerDkMunicipality VejleKommune = new(630, "Vejle");
    public static readonly BorgerDkMunicipality VesthimmerlandsKommune = new(820, "Vesthimmerland", "Vesthimmerlands Kommune");
    public static readonly BorgerDkMunicipality ViborgKommune = new(791, "Viborg");
    public static readonly BorgerDkMunicipality VordingborgKommune = new(390, "Vordingborg");
    public static readonly BorgerDkMunicipality AeroeKommune = new(492, "Ærø");
    public static readonly BorgerDkMunicipality AabenraaKommune = new(580, "Aabenraa");
    public static readonly BorgerDkMunicipality AalborgKommune = new(851, "Aalborg");
    public static readonly BorgerDkMunicipality AarhusKommune = new(751, "Aarhus");

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