﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TextMatch.Tests
{
    [TestClass]
    public class FullTextIndexTests
    {
        static IList<string> _apodArticles;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            // Articles from http://apod.nasa.gov (Astronomy Picture of the Day)
            _apodArticles = new List<string>()
            {
                "Dusty debris from periodic Comet Swift-Tuttle was swept up by planet Earth this week. Vaporized by their passage through the dense atmosphere at 59 kilometers per second, the tiny grains produced a stream of Perseid meteors. A bright, colorful Perseid meteor flash was captured during this 20 second exposure. It made its ephemeral appearance after midnight on August 12, in the moonless skies over the broad granite dome of Enchanted Rock State Natural Area, central Texas, USA. Below the Perseid meteor, trees stand in silhouette against scattered lights along the horizon and the faint Milky Way, itself cut by dark clouds of interstellar dust.",
                "While hunting for comets in the skies above 18th century France, astronomer Charles Messier diligently kept a list of the things he encountered that were definitely not comets. This is number 27 on his now famous not-a-comet list. In fact, 21st century astronomers would identify it as a planetary nebula, but it's not a planet either, even though it may appear round and planet-like in a small telescope. Messier 27 (M27) is an excellent example of a gaseous emission nebula created as a sun-like star runs out of nuclear fuel in its core. The nebula forms as the star's outer layers are expelled into space, with a visible glow generated by atoms excited by the dying star's intense but invisible ultraviolet light. Known by the popular name of the Dumbbell Nebula, the beautifully symmetric interstellar gas cloud is over 2.5 light-years across and about 1,200 light-years away in the constellation Vulpecula. This impressive color composite highlights details within the well-studied central region and fainter, seldom imaged features in the nebula's outer halo. It incorporates broad and narrowband images recorded using filters sensitive to emission from sulfur, hydrogen and oxygen atoms.",
                "These three bright nebulae are often featured in telescopic tours of the constellation Sagittarius and the crowded starfields of the central Milky Way. In fact, 18th century cosmic tourist Charles Messier cataloged two of them; M8, the large nebula left of center, and colorful M20 on the right. The third, NGC 6559, is above M8, separated from the larger nebula by a dark dust lane. All three are stellar nurseries about five thousand light-years or so distant. The expansive M8, over a hundred light-years across, is also known as the Lagoon Nebula. M20's popular moniker is the Trifid. Glowing hydrogen gas creates the dominant red color of the emission nebulae, with contrasting blue hues, most striking in the Trifid, due to dust reflected starlight. The colorful skyscape recorded with telescope and digital camera also includes one of Messier's open star clusters, M21, just above the Trifid.",
                "Very faint but also very large on planet Earth's sky, a giant Squid Nebula cataloged as Ou4, and Sh2-129 also known as the Flying Bat Nebula, are both caught in this scene toward the royal constellation Cepheus. Composed with a total of 20 hours of broadband and narrowband data, the telescopic field of view is almost 4 degrees or 8 Full Moons across. Discovered in 2011 by French astro-imager Nicolas Outters, the Squid Nebula's alluring bipolar shape is distinguished here by the telltale blue-green emission from doubly ionized oxygen atoms. Though apparently completely surrounded by the reddish hydrogen emission region Sh2-129, the true distance and nature of the Squid Nebula have been difficult to determine. Still, a recent investigation suggests Ou4 really does lie within Sh2-129 some 2,300 light-years away. Consistent with that scenario, Ou4 would represent a spectacular outflow driven by a triple system of hot, massive stars, cataloged as HR8119, seen near the center of the nebula. If so, the truly giant Squid Nebula would physically be nearly 50 light-years across.",
                "The delightful Dark Doodad Nebula drifts through southern skies, a tantalizing target for binoculars in the constellation Musca, The Fly. The dusty cosmic cloud is seen against rich starfields just south of the prominent Coalsack Nebula and the Southern Cross. Stretching for about 3 degrees across this scene the Dark Doodad is punctuated at its southern tip (lower left) by globular star cluster NGC 4372. Of course NGC 4372 roams the halo of our Milky Way Galaxy, a background object some 20,000 light-years away and only by chance along our line-of-sight to the Dark Doodad. The Dark Doodad's well defined silhouette belongs to the Musca molecular cloud, but its better known alliterative moniker was first coined by astro-imager and writer Dennis di Cicco in 1986 while observing Comet Halley from the Australian outback.",
                "An old Moon and the stars of Orion rose above the eastern horizon on August 10. The Moon's waning crescent was still bright enough to be overexposed in this snapshot taken from another large satellite of planet Earth, the International Space Station. A greenish airglow traces the atmosphere above the limb of the planet's night. Below, city lights and lightning flashes from thunderstorms appear over southern Mexico. The snapshot also captures the startling apparition of a rare form of upper atmospheric lightning, a large red sprite caught above a lightning flash at the far right. While the space station's orbital motion causes the city lights to blur and trail during the exposure, the extremely brief flash of the red sprite is sharp. Now known to be associated with thunderstorms, much remains a mystery about sprites including how they occur, their effect on the atmospheric global electric circuit, and if they are somehow related to other upper atmospheric lightning phenomena such as blue jets or terrestrial gamma flashes.",
                "After sunset on September 1, an exceptionally intense, reddish airglow flooded this Chilean winter night skyscape. Above a sea of clouds and flanking the celestial Milky Way, the airglow seems to ripple and flow across the northern horizon in atmospheric waves. Originating at an altitude similar to aurorae, the luminous airglow is instead due to chemiluminescence, the production of light through chemical excitation. Commonly captured with a greenish tinge by sensitive digital cameras, this reddish airglow emission is from OH molecules and oxygen atoms at extremely low densities and has often been present in southern hemisphere nights during the last few years. On this night it was visible to the eye, but seen without color. Antares and the central Milky Way lie near the top, with bright star Arcturus at left. Straddling the Milky Way close to the horizon are Vega, Deneb, and Altair, known in northern nights as the stars of the Summer Triangle.",
                "Despite appearances, the sky is not falling. Two weeks ago, however, tiny bits of comet dust were. Featured here is the Perseids meteor shower as captured over Mt. Rainier, Washington, USA. The image was created from a two-hour time lapse video, snaring over 20 meteors, including one that brightened dramatically on the image left. Although each meteor train typically lasts less than a second, the camera was able to capture their color progressions as they disintegrated in the Earth's atmosphere. Here an initial green tint may be indicative of small amounts of glowing magnesium atoms that were knocked off the meteor by atoms in the Earth's atmosphere. To cap things off, the central band of our Milky Way Galaxy was simultaneously photographed rising straight up behind the snow-covered peak of Mt. Rainier. Another good meteor shower is expected in mid-November when debris from a different comet intersects Earth as the Leonids.",
                "There is no sea on Earth large enough to contain the Shark nebula. This predator apparition poses us no danger, though, as it is composed only of interstellar gas and dust. Dark dust like that featured here is somewhat like cigarette smoke and created in the cool atmospheres of giant stars. After being expelled with gas and gravitationally recondensing, massive stars may carve intricate structures into their birth cloud using their high energy light and fast stellar winds as sculpting tools. The heat they generate evaporates the murky molecular cloud as well as causing ambient hydrogen gas to disperse and glow red. During disintegration, we humans can enjoy imagining these great clouds as common icons, like we do for water clouds on Earth. Including smaller dust nebulae such as Lynds Dark Nebula 1235 and Van den Bergh 149 & 150, the Shark nebula spans about 15 light years and lies about 650 light years away toward the constellation of the King of Aethiopia (Cepheus).",
                "Astronomers turn detectives when trying to figure out the cause of startling sights like NGC 1316. Their investigation indicates that NGC 1316 is an enormous elliptical galaxy that started, about 100 million years ago, to devour a smaller spiral galaxy neighbor, NGC 1317, just above it. Supporting evidence includes the dark dust lanes characteristic of a spiral galaxy, and faint swirls and shells of stars and gas visible in this wide and deep image. One thing that remains unexplained is the unusually small globular star clusters, seen as faint dots on the image. Most elliptical galaxies have more and brighter globular clusters than NGC 1316. Yet the observed globulars are too old to have been created by the recent spiral collision. One hypothesis is that these globulars survive from an even earlier galaxy that was subsumed into NGC 1316. Another surprising attribute of NGC 1316, also known as Fornax A, is its giant lobes of gas that glow brightly in radio waves.",
                "The 16th century Portuguese navigator Ferdinand Magellan and his crew had plenty of time to study the southern sky during the first circumnavigation of planet Earth. As a result, two fuzzy cloud-like objects easily visible to southern hemisphere skygazers are known as the Clouds of Magellan, now understood to be satellite galaxies of our much larger, spiral Milky Way galaxy. About 160,000 light-years distant in the constellation Dorado, the Large Magellanic Cloud (LMC) is seen here in a remarkably deep, colorful, image. Spanning about 15,000 light-years or so, it is the most massive of the Milky Way's satellite galaxies and is the home of the closest supernova in modern times, SN 1987A. The prominent patch below center is 30 Doradus, also known as the magnificent Tarantula Nebula, is a giant star-forming region about 1,000 light-years across.",
                "This is the mess that is left when a star explodes. The Crab Nebula, the result of a supernova seen in 1054 AD, is filled with mysterious filaments. The filaments are not only tremendously complex, but appear to have less mass than expelled in the original supernova and a higher speed than expected from a free explosion. The featured image, taken by the Hubble Space Telescope, is presented in three colors chosen for scientific interest. The Crab Nebula spans about 10 light-years. In the nebula's very center lies a pulsar: a neutron star as massive as the Sun but with only the size of a small town. The Crab Pulsar rotates about 30 times each second.",
                "What are those strange blue objects? Many of the brightest blue images are of a single, unusual, beaded, blue, ring-like galaxy which just happens to line-up behind a giant cluster of galaxies. Cluster galaxies here typically appear yellow and -- together with the cluster's dark matter -- act as a gravitational lens. A gravitational lens can create several images of background galaxies, analogous to the many points of light one would see while looking through a wine glass at a distant street light. The distinctive shape of this background galaxy -- which is probably just forming -- has allowed astronomers to deduce that it has separate images at 4, 10, 11, and 12 o'clock, from the center of the cluster. A blue smudge near the cluster center is likely another image of the same background galaxy. In all, a recent analysis postulated that at least 33 images of 11 separate background galaxies are discernable. This spectacular photo of galaxy cluster CL0024+1654 from the Hubble Space Telescope was taken in November 2004.",
                "After grazing the western horizon on northern summer evenings Comet PanSTARRS (also known as C/2014 Q1) climbed higher in southern winter skies. A visitor to the inner Solar System discovered in August 2014 by the prolific panSTARRS survey, the comet was captured here on July 17. Comet and colorful tails were imaged from Home Observatory in Mackay, Queensland, Australia. The field of view spans just over 1 degree. Sweeping quickly across a the sky this comet PanSTARRS was closest to planet Earth about 2 days later. Still, the faint stars of the constellation Cancer left short trails in the telescopic image aligned to track the comet's rapid motion. PanSTARRS' bluish ion tails stream away from the Sun, buffetted by the solar wind. Driven by the pressure of sunlight, its more diffuse yellowish dust tail is pushed outward and lags behind the comet's orbit. A good target for binoculars from southern latitudes, in the next few days the comet will sweep through skies near Venus, Jupiter, and bright star Regulus.",
                "It has been one of the better skies of this long night. In parts of Antarctica, not only is it winter, but the Sun can spend weeks below the horizon. At China's Zhongshan Station, people sometimes venture out into the cold to photograph a spectacular night sky. The featured image from one such outing was taken in mid-July, just before the end of this polar night. Pointing up, the wide angle lens captured not only the ground at the bottom, but at the top as well. In the foreground is a colleague also taking pictures. In the distance, a spherical satellite receiver and several windmills are visible. Numerous stars dot the night sky, including Sirius and Canopus. Far in the background, stretching overhead from horizon to horizon, is the central band of our Milky Way Galaxy. Even further in the distance, visible as extended smudges near the top, are the Large and Small Magellanic Clouds, satellite galaxies near our huge Milky Way Galaxy.",
                "Will Comet Catalina become visible to the unaided eye? Given the unpredictability of comets, no one can say for sure, but it seems like a good bet. The comet was discovered in 2013 by observations of the Catalina Sky Survey. Since then, Comet C/2013 US10 (Catalina) has steadily brightened and is currently brighter than 8th magnitude, making it visible with binoculars and long-duration camera images. As the comet further approaches the inner Solar System it will surely continue to intensify, possibly becoming a naked eye object sometime in October and peaking sometime in late November. The comet will reside primarily in the skies of the southern hemisphere until mid-December, at which time its highly inclined orbit will bring it quickly into northern skies. Featured above, Comet Catalina was imaged last week sporting a green coma and two growing tails.",
                "The Moon was new on July 16. Its familiar nearside facing the surface of planet Earth was in shadow. But on that date a million miles away, the Deep Space Climate Observatory (DSCOVR) spacecraft's Earth Polychromatic Imaging Camera (EPIC) captured this view of an apparently Full Moon crossing in front of a Full Earth. In fact, seen from the spacecraft's position beyond the Moon's orbit and between Earth and Sun, the fully illuminated lunar hemisphere is the less familiar farside. Only known since the dawn of the space age, the farside is mostly devoid of dark lunar maria that sprawl across the Moon's perpetual Earth-facing hemisphere. Only the small dark spot of the farside's Mare Moscoviense (Sea of Moscow) is clear, at the upper left. Planet Earth's north pole is near 11 o'clock, with the North America visited by Hurricane Dolores near center. Slight color shifts are visible around the lunar edge, an artifact of the Moon's motion through the field caused by combining the camera's separate exposures taken in quick succession through different color filters. While monitoring the Earth and solar wind for space weather forcasts, about twice a year DSCOVR can capture similar images of Moon and Earth together as it crosses the orbital plane of the Moon.",
                "That is not our Moon. It's Dione, and it’s a moon of Saturn. The robotic Cassini spacecraft took the featured image during a flyby of Saturn's cratered Moon last month. Perhaps what makes this image so interesting, though, is the background. First, the large orb looming behind Dione is Saturn itself, faintly lit by sunlight first reflected from the rings. Next, the thin lines running diagonally across the image are the rings of Saturn themselves. The millions of icy rocks that compose Saturn's spectacular rings all orbit Saturn in the same plane, and so appear surprisingly thin when seen nearly edge-on. Front and center, Dione appears in crescent phase, partially lit by the Sun that is off to the lower left. A careful inspection of the ring plane should also locate the moon Enceladus on the upper right.",
                "Located 20,000 light-years away in the constellation Carina, the young cluster and starforming region Westerlund 2 fills this cosmic scene. Captured with Hubble's cameras in near-infrared and visible light, the stunning image is a celebration of the 25th anniversary of the launch of the Hubble Space Telescope on April 24, 1990. The cluster's dense concentration of luminous, massive stars is about 10 light-years across. Strong winds and radiation from those massive young stars have sculpted and shaped the region's gas and dust, into starforming pillars that point back to the central cluster. Red dots surrounding the bright stars are the cluster's faint newborn stars, still within their natal gas and dust cocoons. But brighter blue stars scattered around are likely not in the Westerlund 2 cluster and instead lie in the foreground of the Hubble anniversary field of view.",
                "Hot blue stars shine brightly in this beautiful, recently formed galactic or \"open\" star cluster. Open cluster NGC 3293 is located in the constellation Carina, lies at a distance of about 8000 light years, and has a particularly high abundance of these young bright stars. A study of NGC 3293 implies that the blue stars are only about 6 million years old, whereas the cluster's dimmer, redder stars appear to be about 20 million years old. If true, star formation in this open cluster took at least 15 million years. Even this amount of time is short, however, when compared with the billions of years stars like our Sun live, and the over-ten billion year lifetimes of many galaxies and our universe. Pictured, NGC 3293 appears just in front of a dense dust lane and red glowing hydrogen gas emanating from the Carina Nebula."
            };
            
        }

        [TestMethod]
        public void Can_Use_FullTextIndex_Directly()
        {
            // First we create an instance of FullTextIndex
            var index = new FullTextIndex();

            // Then we add texts to the index
            index.Add(_apodArticles);

            // After adding we can Search for texts that match a Lucene query expression
            // See https://lucene.apache.org/core/2_9_4/queryparsersyntax.html for a reference on Lucene query syntax
            var result =  index.Search("magellanic nebula visible in southern skies").ToList();
            var expected = 10;
            var actual = result.Count > 0 ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);   // Article #10 should come up on top

            result = index.Search("swift-tuttle comet").ToList();
            expected = 0;
            actual = result.Count > 0 ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);    // Article #0 should come up on top

            result = index.Search("china observation station in antartica").ToList();
            expected = 14;
            actual = result.Count > 0 ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);    // Article #14 should come up on top
        }

        [TestMethod]        
        public void Can_Use_Ext_Method_On_List()
        {
            // Here we don't instantiate the FullTextIndex directly,
            // but instead use the FullTextMatch() extension method.         

            var result = _apodArticles.Match("magellanic nebula visible in southern skies");
            var expected = 10;
            var actual = result.Success ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);   // Article #10 should come up on top

            result = _apodArticles.Match(@"""swift tuttle"" AND comet");
            expected = 0;
            actual = result.Success ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);    // Article #0 should come up on top

            result = _apodArticles.Match("china observation station in antartica");
            expected = 14;
            actual = result.Success ? result[0] : -1;

            Assert.AreEqual<int>(expected, actual);    // Article #14 should come up on top
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidQueryException))]
        public void Invalid_Query_Produces_Exception()
        {
            // The below query will fail with an InvalidQueryExpression because of
            // the open square bracket.
            var result = _apodArticles.Match("this [is invalid because of the open square bracket");
            var expected = false;
            var actual = result.Success;

            Assert.AreEqual<bool>(expected, actual);   
        }

        [TestMethod]
        public void Can_Tokenize_Text()
        {            
            var textTokens = FullTextIndex.Tokenize(_apodArticles[0]);
            var queryTokens = FullTextIndex.Tokenize(@"""swift tuttle"" AND comet");

            // The text and the query must have tokenized words in common
            // in order for them to match.
            var matchingTerms = textTokens.Intersect(queryTokens); 

            Assert.IsTrue(matchingTerms.Count() > 0);
        }

        [TestMethod]
        public void Words_Are_Stemmed()
        {
            // Stemming minimizes variations in words due to tense (i.e. past, present, future), pluralization, etc.
            // We use the Porter stemming algorithm, which is for English text.
            var text = "While jogging last night, I saw rocketships streaking across the dark moonless sky - at five times the speed of sound!!!";
            var tokens = FullTextIndex.Tokenize(text);

            var expected = "while jog last night i saw rocketship streak across the dark moonless sky at five time the speed of sound";
            var actual = String.Join(" ", tokens.ToArray());

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Can_Use_Proximity_Search()
        {
            // Use proximity search to search for occurrence of words within N words from each other
            var text = "While jogging last night, I saw a rocketship streaking across the dark moonless sky - at five times the speed of sound!!!";
            var query = @"""rocketship dark sky""~4";

            var result = new[] { text }.Match(query);
            var expected = 0;
            var actual = result.Success ? result[0] : -1;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Can_Match_Multiple_Expressions_To_Single_Text()
        {
            // We can have one text and match it against a list of query expressions
            var text = "While jogging last night, I saw a rocketship streaking across the dark moonless sky - at five times the speed of sound!!!";        
            var queryExpressions = new[]
            {
                @"""rocketship dark sky""~4",
                "jogging at night",
                "speed of sound",
                "(meteor shower) OR perseid"
            };

            var result = text.Match(queryExpressions);
            var expected = new [] { 0, 1, 2 };
            var actual = result;


            Assert.IsTrue(expected.SequenceEqual(actual));
        }

        [TestMethod]
        public void Can_Match_Multiple_Expressions_To_Multiple_Texts_NonCached()
        {
            var queryExpressions = new[]
            {
                @"""rocketship dark sky""",
                @"""starry night""",
                @"""sound barrier""",
                @"""meteor shower""~2 OR perseid",
                @"""horse with no name""",
                @"""magellanic clouds"" AND ""star formation""",
                @"""background radiation""",
                @"""hubble space telescope""",
                @"""saturn white spot""~2",
                @"""distance to andromeda""",
                @"""chinese space observatory"""
            };


            var multiResults = _apodArticles.Match(queryExpressions, cacheQuery: false);

            for (var i = 0; i < _apodArticles.Count; i++)
            {
                var text = _apodArticles[i];
                var expected = text.Match(queryExpressions);
                var actual = multiResults[i];

                Assert.IsTrue(expected.SequenceEqual(actual));
            }
        }


        [TestMethod]
        public void Can_Match_Multiple_Expressions_To_Multiple_Texts_Cached()
        {
            var queryExpressions = new[]
            {
                @"""rocketship dark sky""",
                @"""starry night""",
                @"""sound barrier""",
                @"""meteor shower""~2 OR perseid",
                @"""horse with no name""",
                @"""magellanic clouds"" AND ""star formation""",
                @"""background radiation""",
                @"""hubble space telescope""",
                @"""saturn white spot""~2",
                @"""distance to andromeda""",
                @"""chinese space observatory"""
            };


            var multiResults = _apodArticles.Match(queryExpressions, cacheQuery: true);

            for (var i = 0; i < _apodArticles.Count; i++)
            {
                var text = _apodArticles[i];
                var expected = text.Match(queryExpressions);
                var actual = multiResults[i];

                Assert.IsTrue(expected.SequenceEqual(actual));
            }
        }

        [TestMethod]
        public void Can_Match_Using_Fuzzy_Search()
        {
            // We want to search for texts containing words that are close in spelling to "streak"
            // this will match "stream"
            var result = _apodArticles.Match("streak~");     
            var expected = new[] { 0, 13, 12 };    // The expression matches articles #0, #13, and #12
            var actual = result;

            Assert.IsTrue(expected.SequenceEqual(actual));

        }


        [TestMethod]
        public void Can_Match_Using_Wildcards()
        {
            // We want to search for texts that start with black            
            var result = _apodArticles.Match("space AND explo*");
            var expected = new[] { 11 };    // The expression matches article #11
            var actual = result;

            Assert.IsTrue(expected.SequenceEqual(actual));

        }
    }
}
