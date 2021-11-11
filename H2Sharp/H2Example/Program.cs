using System;
using System.Data;
using System.Data.H2;
using System.Diagnostics;

namespace H2Example
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var connection = new H2Connection("jdbc:h2:mem:db1;DB_CLOSE_DELAY=-1;");
            connection.Open();
            new H2Command("create table list (item integer primary key, description varchar(256), value integer)", connection).ExecuteNonQuery();
            new H2Command("insert into list values (1, 'First Item', 10)", connection).ExecuteNonQuery();
            new H2Command("insert into list values (2, 'Second Item', 11)", connection).ExecuteNonQuery();

            var table = new DataTable("test")
            {
                CaseSensitive = false
            };
            var adapter = new H2DataAdapter("select item, description, value from list order by item", connection);
            new H2CommandBuilder(adapter);
            adapter.Fill(table);

            table.Rows[1][1] = "First item modified";
            table.Rows[1][2] = 12;
            table.Rows.Add(new object[] { 3, "Third", 15 });
            adapter.Update(table);

            var table2 = new DataTable("test")
            {
                CaseSensitive = false
            };
            adapter.Fill(table2);

            var x1 = table.ToXml();
            var x2 = table2.ToXml();
            Debug.Assert(x1.Equals(x2));

            var count = new H2Command("select count(*) from list", connection).ExecuteScalar();
            Debug.Assert(((long)count).Equals(3));

            var one = new H2Command("select 1 from dual", connection).ExecuteScalar();
            Debug.Assert(((int)one).Equals(1));

            var a = new H2Command("select 'a' from dual", connection).ExecuteScalar();
            Debug.Assert(((string)a).Equals("a"));

            var p = new H2Command("select /*fff*/-- ddd\r\n 'a' from dual where 1=:p0 and 2=2", connection);
            java.lang.Integer iq = java.lang.Integer.decode("1");
            p.Parameters.Add(new H2Parameter("p0", iq));
            var aa = p.ExecuteScalar();
            Debug.Assert(((string)aa).Equals("a"));
            var pp = new H2Command(@"--CREATE SCHEMA ocn_btc
                --    AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_cfg
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_clr
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_iss
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_acq
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_trash
                --AUTHORIZATION postgres;

                            --CREATE SCHEMA ocn_cat
                --AUTHORIZATION postgres;

                            /*
                            CREATE OR REPLACE FUNCTION OCN_CFG.FN_GET_LANGUAGE_TEXT (
                               INPUT      IN VARCHAR,
                               LANGUAGE   IN VARCHAR)
                               RETURNS VARCHAR AS $L_LANGUAGE_TEXT$
                            DECLARE
                               L_LANGUAGE        VARCHAR := LANGUAGE;
                               L_LANGUAGE_TEXT   VARCHAR ;
                               L_DEFAULT_LANG    VARCHAR := 'en-US';
                            BEGIN
                               IF    INPUT IS NULL
                                  OR LENGTH (TRIM (INPUT)) = 0
                                  OR L_LANGUAGE IS NULL
                                  OR LENGTH (TRIM (L_LANGUAGE)) = 0
                               THEN
                                  RETURN INPUT;
                               END IF;

                               IF POSITION (L_LANGUAGE || ':=' IN INPUT) = 0
                               THEN
                                  L_LANGUAGE := UPPER(SUBSTRING(L_LANGUAGE FROM 1 FOR 2));
                                  IF POSITION (L_LANGUAGE|| ':=' IN INPUT) = 0
                                  THEN
                                      L_LANGUAGE := L_DEFAULT_LANG;
                                      IF POSITION (L_LANGUAGE || ':=' IN INPUT) = 0
                                      THEN
                                        L_LANGUAGE := UPPER(SUBSTRING(L_LANGUAGE FROM 1 FOR 2));
                                        IF POSITION (L_LANGUAGE|| ':=' IN INPUT) = 0
                                        THEN
                                             RETURN INPUT;
                                        END IF;
                                      END IF;
                                  END IF;
                               END IF;

                               L_LANGUAGE_TEXT :=
                                  SUBSTRING (INPUT FROM
                                          POSITION (L_LANGUAGE || ':=' IN INPUT) + LENGTH (l_language) + 2);

                               IF POSITION (':='  IN L_LANGUAGE_TEXT) > 0
                               THEN
                                  L_LANGUAGE_TEXT :=
                                     SUBSTRING (L_LANGUAGE_TEXT FROM 1 FOR POSITION (';;' IN L_LANGUAGE_TEXT) - 1);
                               END IF;

                               RETURN L_LANGUAGE_TEXT;
                            EXCEPTION
                               WHEN OTHERS
                               THEN
                                  raise;
                            END;
                            $L_LANGUAGE_TEXT$ LANGUAGE plpgsql;
                            */
                            ", connection);
            var aba = pp.ExecuteScalar();

            var ppsx = new H2Command(@"CREATE SCHEMA IF NOT EXISTS ocn_cfg;
create table OCN_CFG.CFG_BRANCH_DEF(GUID NUMBER(20, 0) not null, LAST_UPDATED NUMBER(20, 0) not null, MBR_ID NUMBER(5, 0), CITY_CODE VARCHAR2(255), TOWN_CODE VARCHAR2(255), BRANCH_TYPE VARCHAR2(255), COUNTRY_CODE VARCHAR2(255), CODE NUMBER(20, 0) unique, DESCRIPTION VARCHAR2(80), ADDRESS1 VARCHAR2(50), ADDRESS2 VARCHAR2(50), ZIP_CODE VARCHAR2(5), IS_CYPRUS_BRANCH NUMBER(5, 0), IS_VALID NUMBER(5, 0), BRANCH_ADMIN VARCHAR2(40), PHONE1 VARCHAR2(14), primary key(GUID));
            ", connection);
            var abix = ppsx.ExecuteNonQuery();

            var pps = new H2Command(@"INSERT INTO OCN_CFG.CFG_BRANCH_DEF (guid,last_updated,mbr_id,code,description,address1,address2,country_code,city_code,town_code,zip_code,is_cyprus_branch,is_valid,branch_admin,branch_type,phone1) VALUES	 (1000000003541698,1,1,85165,'YURTPA-BEKO-ERZ?NCAN','CUMHUR?YET MH SÜLEYMAN DEM ?REL CD NO:8',NULL,'792','024','2401',NULL,0,0,'M?KA?L CANPOLAT','B',NULL),	 (1000000003541699,1,1,34109,'KIBRIS T?CARET-ARÇEL?K-NATAVEG','NATAVEGA A.V.M',NULL,'792','006','0619',NULL,0,0,'HÜSEY?N BALCI','B',NULL),	 (1000000003541700,1,1,9340547,'HAS AY TUR?ZM TA?IMACILIK T?C.','YUKARI MAH. ÜSKÜDAR CAD','KARDE?LER ?? HANI NO:28','792','034','3414',NULL,0,0,'HAS AY TUR?ZM TA?IMACILIK','B',NULL),	 (1000000003541701,1,1,9430012,'DAVUT KARAKURT','CERELLER MAH.BA?ARAN ULUSOY CAD NO:2',NULL,'792','043','4312',NULL,0,0,'DAVUT KARAKURT','B',NULL),	 (1000000003541702,1,1,83999,'YUSUF UYAK-ERCAN HOME','HAF?Z?YE MAH. KAZIM KARABEK?R BULVARI','NO:130/1 ?PEKYOLU-VAN','792','065','6500',NULL,0,0,'YUSUF UYAK','B',NULL),	 (1000000003541703,1,1,80697,'?ENOL DAYANIKLI-BE??KDÜZÜ BEKO','CUMHUR?YET MAH. MEYDAN CAD. NO:22',NULL,'792','061','6105',NULL,0,0,'AYSUN DEM?RC?','B',NULL),	 (1000000003541704,1,1,9340043,'SÜLEYMAN ?NAN','SANAY? MAH. ATATÜRK CAD. NO.80',NULL,'792','034','3429',NULL,0,0,'SÜLEYMAN ?NAN','B',NULL),	 (1000000003541705,1,1,90466,'MERS?N ?UBE-ADAB?R TEKST?L','E??RÇAM MAH GMK BULVARI N.560A YEN??EH?R',NULL,'792','033','1613',NULL,0,0,'MURAT SAYIR','B',NULL),	 (1000000003541706,1,1,72890,'RAMAZAN AYDEM?R','ATAKENT MAH. 2.ETAP KU?LAR TEPES? CAD. ','NO:1 T2 ?? MERKEZ?','792','034','3415',NULL,0,0,'RAMAZAN AYDEM?R','B',NULL),	 (1000000003541707,1,1,2504048,'P?R-AN BEKO-BEKO','HACI HALIL MAH ATATURK CAD','1221 SK KISACIK IS MRK.','792','041','4102',NULL,0,0,'SERVET GÜNE?','B',NULL);
INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541708, 1, 1, 86694, 'SUNGURLU IN?AAT', 'AYDINLIKEVLER MAH.?RFAN BA?TU? CAD', 'NO:72/A', '792', '006', '1130', NULL, 0, 0, 'AL? ALSAÇ', 'B', NULL), (1000000003541709, 1, 1, 9340157, 'TAYBET ARIK', 'YUKARI DUDULLU MAH.BOSTANCI YOLU CAD. ', 'NO:28 / B  ASYA PARK ALI? VER?? MERKEZ?', '792', '034', '3421', NULL, 0, 0, 'TAYBET ARIK', 'B', NULL), (1000000003541710, 1, 1, 70056, 'S?MBAT AKYOL', 'OBA MAH. OER-ERKENSCW?CK CAD. ÇALI? APT.', 'NO:11/A', '792', '007', '0703', NULL, 0, 0, 'S?MBAT AKYOL', 'B', NULL), (1000000003541711, 1, 1, 83994, 'ÖZGÖNÜL MOBILYA EVTEKS-?UBE', 'ÖZALPER MH. ÖZLAP SK. NO:2', NULL, '792', '044', '4400', NULL, 0, 0, 'HÜLYA EREN', 'B', NULL), (1000000003541712, 1, 1, 9340035, 'FURKAN DANI?MANLIK', 'ÇINAR MAH. 5/3. SOKAK NO.9/B', NULL, '792', '034', '3427', NULL, 0, 0, 'SEDAT SA?LIK', 'B', NULL), (1000000003541713, 1, 1, 74722, 'SÜLEYMAN TORAMAN', '21 HAZ?RAN MAH. ESK? BAYBURT CAD. NO:2/2', NULL, '792', '025', '2517', NULL, 0, 0, 'SÜLEYMAN TORAMAN', 'B', NULL), (1000000003541714, 1, 1, 23424, 'GUSTO T?CARET-YATA?', 'GAZ? OSMAN PA?A MAH. ?EH?T GAFFAR OKKAN', 'CAD. NO:82/C-D-E-F GÖLBA?I/ANKARA', '792', '006', '0205', NULL, 0, 0, 'MURAT KARA', 'B', NULL), (1000000003541715, 1, 1, 79470, 'BEKTA? SAK', 'HANÇERL? MAH. 100.YIL BLV. NO: 15-1 ', '?LKADIM', '792', '055', '5501', NULL, 0, 0, 'BEKTA? SAK', 'B', NULL), (1000000003541716, 1, 1, 9070079, 'MANAVGAT GÜLLÜK', 'A?A?IH?SAR MAH.GÜLLÜK CAD.', 'NO:19 ', '792', '007', '0712', NULL, 0, 0, 'BURAK TONTUL', 'B', NULL), (1000000003541717, 1, 1, 9550016, 'NUMAN ÜNLÜ', 'YEN? MAH. BEYAZIT  CAD. NO:14/A', NULL, '792', '019', '0303', NULL, 0, 0, 'NUMAN ÜNLÜ', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541718, 1, 1, 9340711, 'CEM?L ÇEL?KKAYA', 'CENNET MAH. MEVLANA CAD. NO:22/A', NULL, '792', '034', '3415', NULL, 0, 0, 'CEM?L ÇEL?KKAYA', 'B', NULL),	 (1000000003541719, 1, 1, 72797, 'ENG?N ÖLMEZ', 'KARADOLAP MAH.VEYSEL KARAN? CAD..NO:10/B', NULL, '792', '034', '3409', NULL, 0, 0, 'ENG?N ÖLMEZ', 'B', NULL),	 (1000000003541720, 1, 1, 77171, 'NECMETT?N Y???T-SERKAN Y???T ', '?STOÇ OTO MARKET R BLOK NO:5', NULL, '792', '034', '3427', NULL, 0, 0, 'NECMETT?N Y?G?T', 'B', NULL),	 (1000000003541721, 1, 1, 28371, 'ATAÜN DTM-ARÇEL?K', 'SULTAN SEL?M MH.GAZ? CADDES? N:29/A', NULL, '792', '028', '2800', NULL, 0, 0, 'GÜL AY?E DEM?RAL', 'B', NULL),	 (1000000003541722, 1, 1, 46307, 'CANAN ÖZTÜRE-BOSCH', 'MALTEPE MH. KAYMAKAM KEMALBEY CAD. NO:25', NULL, '792', '035', '3504', NULL, 0, 0, 'CANAN ÖZTÜRE', 'B', NULL),	 (1000000003541723, 1, 1, 9430011, 'AL? BALCI-FATURA ÖDEME MERKEZI', 'CUMHUR?YET MAH.ÇALKÖY CAD.NO:1', NULL, '792', '043', '4303', NULL, 0, 0, 'AL? BALCI', 'B', NULL),	 (1000000003541724, 1, 1, 22813, 'DETAY GRUP-DO?TA?', 'ZAFER CAD BELED?YE SOK NO:3/3 KIRIKKALE', NULL, '792', '071', '7100', NULL, 0, 0, 'MURAT BOZKURT', 'B', NULL),	 (1000000003541725, 1, 1, 73328, 'ÇVK YAPI GRUP MOB D??ER', 'ÇINARDERE MAH ANKARA CAD NO:67', NULL, '792', '034', '3416', NULL, 0, 0, 'ÖMER ?LBAY', 'B', NULL),	 (1000000003541726, 1, 1, 60969, 'AFS BEKO', 'BAHÇEL?EVLER MAH.CUMHUR?YET CAD. 39 B', NULL, '792', '002', '0200', NULL, 0, 0, 'AB?D?N ÇEL?K', 'B', NULL),	 (1000000003541727, 1, 1, 79122, 'ÖZCANLAR ?PEK MOB. ?B.', '?STASYON MAH. BOSNA CAD. 1/11-1-2', NULL, '792', '006', '0622', NULL, 0, 0, 'YEL?Z TAYLAN', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541728, 1, 1, 32307, 'KARACALAR-ARÇEL?K-NEV?EH?R', 'ATATÜRK BUL.N0:51/B', NULL, '792', '050', '5001', NULL, 0, 0, 'AHMET ERKAN KARACA', 'B', NULL),	 (1000000003541729, 1, 1, 78809, 'ÇÖZÜM PAZARLAMA-MERZ?FON', 'HARMANLAR HARMANLAR CADDES? 17 AMERZ?FON', NULL, '792', '005', '0504', NULL, 0, 0, 'FEYZA ANIL', 'B', NULL),	 (1000000003541730, 1, 1, 69525, 'OTOZEN OTOMOT?V', '100.YIL MH.VEYSEL KARAN? CAD', 'AUTOMAL GA C-BZ 38', '792', '034', '3427', NULL, 0, 0, 'UFUK YAVUZ', 'B', NULL),	 (1000000003541731, 1, 1, 80428, 'ORHAN ERGÜL ?UBE-BOZÜYÜK', 'YEN? MAH ?SMET ?NÖNÜ MH NO:122 A', 'BOZÜYÜK', '792', '011', '1192', NULL, 0, 0, 'C?HAN ERGÜL', 'B', NULL),	 (1000000003541732, 1, 1, 38320, 'D?R?K ARÇEL?K-OSMANGAZ?', 'BA?LARBA?I MAH M.GENÇO?LU CAD', 'N.58', '792', '016', '1832', NULL, 0, 0, 'RECEP D?R?K', 'B', NULL),	 (1000000003541733, 1, 1, 82594, 'KARDE?LER DAY.TÜK.MAL.', 'PINARBA?I M. VATAN CAD. NO:88/A', NULL, '792', '006', '0622', NULL, 0, 0, 'MUHARREM KURT', 'B', NULL),	 (1000000003541734, 1, 1, 80777, 'ABDULLAH SAC?T T?NAR', 'CAM?KEB?R MAH.BA?AR SK.N.4A/A', NULL, '792', '054', '5403', NULL, 0, 0, 'ABDULLAH SAC?T T?NAR', 'B', NULL),	 (1000000003541735, 1, 1, 70104, 'EF KA GRUP', 'SAADETDERE MAH.SEMT 33.SK.N.8/1', NULL, '792', '034', '3439', NULL, 0, 0, 'FUAT KAYA', 'B', NULL),	 (1000000003541736, 1, 1, 10802, 'GÖKERLER MOB?LYA-?ST?KBAL', 'GAYRET MAH. ?VED?K CAD. NO:73', NULL, '792', '006', '0625', NULL, 0, 0, 'DAVUT PEKER', 'B', NULL),	 (1000000003541737, 1, 1, 79168, 'KAAN MOB?LYA SANAY? ?N?AAT', 'KA?ÜSTÜ MH DEVLET KARAYOLU CD NO:81/A/C', NULL, '792', '061', '6113', NULL, 0, 0, 'ZAFER OFLUO?LU', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541738, 1, 1, 85852, 'YAZICILAR OTOMOTIV', 'SO?ANLI MAH. Y.YALOVA YEN? OTO.MERKEZ? ', '108 SK NO:10', '792', '016', '1832', NULL, 0, 0, 'C?HAD YAZICI', 'B', NULL),	 (1000000003541739, 1, 1, 80018, 'NEV?EH?RL?LER OTOMOT?V ?N?.TUR', 'YÜZYIL MH. VEYSEL KARAN? CAD.', 'NO:15 A BLOK Z09', '792', '034', '3427', NULL, 0, 0, 'MURAT UYSAL', 'B', NULL),	 (1000000003541740, 1, 1, 74530, 'BERAT MOB?LYA', 'MEHMET AK?F ERSOY MH. GÜL CD. NO:7', NULL, '792', '034', '3431', NULL, 0, 0, 'MEHMET BE??R ALTUN', 'B', NULL),	 (1000000003541741, 1, 1, 9340365, 'EMRAH YALÇIN', 'MURAT ÇE?ME MAH NO:120', 'E5 ÜSTÜ K?LER MARKET ?Ç?', '792', '034', '3406', NULL, 0, 0, 'EMRAH YALÇIN', 'B', NULL),	 (1000000003541742, 1, 1, 48707, 'ASKIN CINAR ASKIN CINAR', 'KADIA?A CAD. NO:64/1', 'M?LAS', '792', '048', '4801', NULL, 0, 0, 'A?KIN ÇINAR', 'B', NULL),	 (1000000003541743, 1, 1, 9340833, 'KA?ITHANE ÖRNEKTEPE', 'MEHMET AK?F ERSOY MAH.ÖRF SK.NO:1/A', NULL, '792', '034', '3413', NULL, 0, 0, '?ZZET YÜZGÜL', 'B', NULL),	 (1000000003541744, 1, 1, 74818, 'GÜVEN E.EV A.?LET H?Z.T?C.LTD.', 'KEMALIYE MAHALLESI, HACI ZIYA HABIBOGLU ', 'CADDESI NO:11/A', '792', '061', '6112', NULL, 0, 0, 'CEM?L GÜVEN', 'B', NULL),	 (1000000003541745, 1, 1, 9340783, 'AY?E YARALI', 'ÇINAR MAH.802.SOKAK NO:72/B', NULL, '792', '034', '3427', NULL, 0, 0, 'AY?E YARALI', 'B', NULL),	 (1000000003541746, 1, 1, 9430002, 'TEK ONL?NE', 'YEN?DO?AN MAH.RAGIPGÜMÜ?PALA CAD.', 'SEVG? SOK.NO:7/A ', '792', '043', '4300', NULL, 0, 0, 'TEK ONL?NE ?LET???M', 'B', NULL),	 (1000000003541747, 1, 1, 46238, 'CEMPA DTM.PAZ.?N?.EMLAK ?TH.?H', 'SOKULLU MEHMET PA?A CD. 34/A  ÇANKAYA', NULL, '792', '006', '0606', NULL, 0, 0, 'YENER CENG?Z', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541748, 1, 1, 9550025, 'KENAN GÜMÜ?', 'ÇINAR MAH. ÜNYE CAD. NO:19', NULL, '792', '052', '5216', NULL, 0, 0, 'KENAN GÜMÜ?', 'B', NULL),	 (1000000003541749, 1, 1, 31253, 'ONUR MOB?LYA-BELLONA', 'GÜNE?TEPE MAH SO?ANLI CAD NO:125', NULL, '792', '034', '3429', NULL, 0, 0, 'SEFA B?RDAL', 'B', NULL),	 (1000000003541750, 1, 1, 88675, 'BURAKLAR OTO-?ZM?R', '371 sok no22/B beyazevler GAZ?EM?R ?ZM?R', NULL, '792', '035', '3526', NULL, 0, 0, 'CEVDET ÖZPINAR', 'B', NULL),	 (1000000003541751, 1, 1, 85183, 'TUNA MOB VARDAR ?B', 'MERKEZ MAH VARDAR BULV NO:28 AL?BEYKÖY', NULL, '792', '034', '3409', NULL, 0, 0, 'REYHAN SABANI', 'B', NULL),	 (1000000003541752, 1, 1, 76147, 'MET?NCAN KURUYEM??', 'CAM??SA?IR MAH.SÜNMAN? CAD. NO:5A', NULL, '792', '025', '2509', NULL, 0, 0, 'ÇET?N AKYOL', 'B', NULL),	 (1000000003541753, 1, 1, 9340819, 'BA?CILAR MERKEZ', 'SANCAKTEPE MAH. 897. SK. ÇA?LAYAN PASAJI', 'NO:19/A ', '792', '034', '3427', NULL, 0, 0, 'HÜSEY?N ?AH?N', 'B', NULL),	 (1000000003541754, 1, 1, 39077, 'ÖKSÜZLER DTM-ARÇEL?K', 'RAS?M EREL CD. OKSUZLER APT. NO:7', NULL, '792', '042', '4212', NULL, 0, 0, 'SÜLEYMAN ÖKSÜZ', 'B', NULL),	 (1000000003541755, 1, 1, 9550048, 'ENDER ?LET???M-?UBE', 'AKB?LEK MAH. ALPTEK?N CAD. ', 'NO: 9', '792', '005', '0500', NULL, 0, 0, 'MEHMET AL? KARAO?LU', 'B', NULL),	 (1000000003541756, 1, 1, 9070003, 'SELMA ACAR', 'MELTEM MAH.MELTEM BULVARI NO:5B/4', NULL, '792', '007', '2039', NULL, 0, 0, 'SELMA ACAR', 'B', NULL),	 (1000000003541757, 1, 1, 88386, 'GBS DTM-ADIYAMAN', 'CUMHUR?YET MAH.HISNI MANSUR 2.ÇEVRE YOLU', 'FAT?H YAPI KOOP.A BLOK 402/A', '792', '002', '0200', NULL, 0, 0, 'GAZ?HAN RIZA ÖZGÜN', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541758, 1, 1, 78317, 'AKIN EV ALETLER? ?N?AAT SANAY?', 'YEN? MH. ?NÖNÜ CD. NO.58/60', NULL, '792', '034', '3415', NULL, 0, 0, 'NECAT? BUDAK', 'B', NULL),	 (1000000003541759, 1, 1, 75491, 'SUAT KELE?', '?EYHAYRAM MAH.DEDEKORKUT CAD.NO:15/A', NULL, '792', '069', '6900', NULL, 0, 0, 'SUAT KELE?', 'B', NULL),	 (1000000003541760, 1, 1, 33844, 'FERA MOB. BEKO. MERKEZ', 'KEMERÇE?ME MAH. KEMERÇE?ME CAD. NO:54', NULL, '792', '016', '1832', NULL, 0, 0, 'HAKAN ?AH?N', 'B', NULL),	 (1000000003541761, 1, 1, 9340653, 'Nejla KÖSEO?LU', 'KAVAKLI MAH.MARMARA CAD. NO : 24/A  ', NULL, '792', '034', '3437', NULL, 0, 0, 'Nejla KÖSEO?LU', 'B', NULL),	 (1000000003541762, 1, 1, 9430018, 'B?LEC?K BOZÜYÜK', 'YEN? MAH.?evket usta sok 11/d ', NULL, '792', '011', '1210', NULL, 0, 0, 'OKAN MUMCU', 'B', NULL),	 (1000000003541763, 1, 1, 34713, 'AYDINLAR T?C-BEKO-KAYA? ?UBE', 'BÜYÜK KAYA? MAH.KAYA? CAD.NO:99', NULL, '792', '006', '0619', NULL, 0, 0, 'NECAT? ÇOBAN', 'B', NULL),	 (1000000003541764, 1, 1, 70459, 'KALB?M MOB?LYA-AYDIN', 'FAT?H MAH 1101 SK 15 1', 'EFELER', '792', '009', '0900', NULL, 0, 0, 'BAHR? BEKEN', 'B', NULL),	 (1000000003541765, 1, 1, 9340608, 'TUNCAY TUNCEL', 'MEHMET AK?F  MAH.A?IK VEYSEL CAD.NO:51/D', NULL, '792', '034', '3415', NULL, 0, 1, 'TUNCAY TUNCEL', 'B', NULL),	 (1000000003541766, 1, 1, 69882, 'MEMDUH D?LBER', 'ÖRNEK MAH.?EH?T CAHAR DUDAYEV CAD.N.42', NULL, '792', '034', '3435', NULL, 0, 0, 'ESMA ÖZDEM?R', 'B', NULL),	 (1000000003541767, 1, 1, 9070061, 'SELMA MUTAF', 'AHATLI MAH. ULUSOY CAD. NO:30/C', NULL, '792', '007', '2037', NULL, 0, 0, 'SELMA MUTAF', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541768, 1, 1, 77938, 'RESUL GED?KO?LU', 'MESC?T MAH.DEMOKRAS? CAD.NO:51/A ', 'ORHANLI TUZLA', '792', '034', '3432', NULL, 0, 0, 'RESUL GED?KO?LU', 'B', NULL),	 (1000000003541769, 1, 1, 9550046, 'BEKTA? SAK', 'HANÇERL? MAH.100. YIL BULVARI ', 'NO:15/1 ?LKADIM', '792', '055', '5501', NULL, 0, 0, 'BEKTA? SAK', 'B', NULL),	 (1000000003541770, 1, 1, 9340790, 'DERYA KARABOZ', 'SULTAN?YE MAH.DO?AN ARASLI BULVARI', 'NO:168/A KENT ?? MERKEZ?', '792', '034', '3439', NULL, 0, 0, 'DERYA KARABOZ', 'B', NULL),	 (1000000003541771, 1, 1, 69380, 'BEYAZ KARDE?LER DTM', 'U.HASAN MAH.KOZAN CAD.N.540/A', NULL, '792', '001', '0117', NULL, 0, 0, 'F?L?Z TOPÇU', 'B', NULL),	 (1000000003541772, 1, 1, 74900, 'NEZAHAT YASLI', 'NUR?PA?A MAH. 14.SOK. NO:84 ', NULL, '792', '034', '3424', NULL, 0, 0, 'NEZAHAT YASLI', 'B', NULL),	 (1000000003541773, 1, 1, 85979, 'FADL? YILDIZ-?UBE', 'BOZOKLAR MAH DEMOKRAS? BULVARI N.152', NULL, '792', '027', '2708', NULL, 0, 0, 'FADL? YILDIZ', 'B', NULL),	 (1000000003541774, 1, 1, 77971, 'BOSCH UZUNO?LU T?C.DAY.TÜK.MAL', 'MAHMUTBEY MH. Ö YILMAZ CD. NO:4', NULL, '792', '054', '5404', NULL, 0, 0, 'ERS?N UZUNO?LU', 'B', NULL),	 (1000000003541775, 1, 1, 9070076, 'ANTALYA ALTINKUM', 'ALTINKUM MAH.428.SK.SEÇ?L APT.NO:14/A', 'KONYAALTI', '792', '007', '4200', NULL, 0, 1, 'ANTALYA ALTINKUM', 'B', NULL),	 (1000000003541776, 1, 1, 9340545, 'ETHEM ?AH?N', 'AYAZA?A MH. ATATÜRK CAD. NO:22/B', NULL, '792', '034', '3420', NULL, 0, 0, 'ETHEM ?AH?N', 'B', NULL),	 (1000000003541777, 1, 1, 47670, 'CETIN KARACANCAY', '?EKER MAH. SEDAT K?RTEPE CAD. NO:59/A ', 'ADAPAZARI', '792', '054', '5401', NULL, 0, 0, 'ÇET?N KARACANÇAY', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541778, 1, 1, 9250025, 'EMRE ATALAY', 'KAR?IYAKA MAH. M?LL? EGEMENL?K CAD. ', 'NO:41', '792', '076', '7602', NULL, 0, 1, 'EMRE ATALAY', 'B', NULL),	 (1000000003541779, 1, 1, 83426, 'HALIS TURAN AYHAN-AYHAN OTO', 'TURGUT RE?S MAH.FAT?H BULVARI 320/a-1', NULL, '792', '034', '3431', NULL, 0, 0, 'HAL?S TURAN AYHAN', 'B', NULL),	 (1000000003541780, 1, 1, 80830, 'NE?E GÜNARSLAN', 'ORTA MAH. ÇAR?I CAD. NO: 31/1', NULL, '792', '034', '3425', NULL, 0, 0, 'NE?E GÜNARSLAN', 'B', NULL),	 (1000000003541781, 1, 1, 73873, 'CEVAT ADIYAMAN', 'KAZIM KARABEK?R MAH.YAVUZ SEL?M CAD.', 'NO:6 ', '792', '025', '2505', NULL, 0, 0, 'CEVAT ADIYAMAN', 'B', NULL),	 (1000000003541782, 1, 1, 34722, 'AYDINLAR T?C-?PEK MOB?LYA', 'DURAL? ALIÇ MAH.DO?UKENT CAD.', 'NO:350/25-26', '792', '006', '0619', NULL, 0, 0, 'MUSTAFA ?AH?N', 'B', NULL),	 (1000000003541783, 1, 1, 9340098, 'TAYFUN YILDIRIM', 'S?YAVU?PA?A MH. FER?T SEL?M PA?A CAD', 'KIZILCIK SK. 13/2 YAYLA', '792', '034', '3428', NULL, 0, 0, 'TAYFUN YILDIRIM', 'B', NULL),	 (1000000003541784, 1, 1, 54249, 'RAPSOD? MOB?LYA-ZEKER?YA SARI', 'YEN? YALOVA YOLU 6.KM NO:427', NULL, '792', '016', '1832', NULL, 0, 0, 'ZEKER?YA SARI', 'B', NULL),	 (1000000003541785, 1, 1, 71889, 'FAD?L EGE', 'GÖKO?LU KÖYÜ NO:59 PATNOS', NULL, '792', '004', '0406', NULL, 0, 0, 'FAD?L EGE', 'B', NULL),	 (1000000003541786, 1, 1, 70223, 'BSB MOB?LYA-GAZ? ?UBE ?ST?KBAL', 'KALE MAH. OSMAN?YE SOK. NO:81', '?LKADIM', '792', '055', '5501', NULL, 0, 0, 'AYNUR KORKMAZ', 'B', NULL),	 (1000000003541787, 1, 1, 74223, 'UMUT ERCAN', 'ESENTEPE MAH.YUNUS EMRE BULVARI NO:5/A ', NULL, '792', '019', '1912', NULL, 0, 0, 'UMUT ERCAN', 'B', NULL);
            INSERT INTO OCN_CFG.CFG_BRANCH_DEF(guid, last_updated, mbr_id, code, description, address1, address2, country_code, city_code, town_code, zip_code, is_cyprus_branch, is_valid, branch_admin, branch_type, phone1) VALUES(1000000003541788, 1, 1, 62933, 'DARICA MOBILYA', 'KAZIM KARABEK?R MH. ?STASYON CAD. NO:439', NULL, '792', '041', '4102', NULL, 0, 0, 'FAHR? YILMAZ', 'B', NULL),	 (1000000003541789, 1, 1, 9717, 'KURTO?LU MOB?LYA -MERKEZ', 'YEN?DO?AN MH. ZAFER CD. NO:69 KIRIKKALE', NULL, '792', '071', '7100', NULL, 0, 0, 'HAVVA EMEL ATALAY', 'B', NULL),	 (1000000003541790, 1, 1, 9260069, 'ESK??EH?R ODUNPAZARI', 'AKÇA?LAN MAH.CUMHUR?YET BLV.NO:6/B', 'ODUNPAZARI', '792', '026', '2600', NULL, 0, 0, 'PEL?N BALCI', 'B', NULL),	 (1000000003541791, 1, 1, 73372, 'YUSUF VARLI', 'MURATPA?A MAH.?ARAMPOL CAD.KARADEN?Z ?? ', 'HANI NO :108Z/06', '792', '007', '2039', NULL, 0, 0, 'YUSUF VARLI', 'B', NULL),	 (1000000003541792, 1, 1, 74505, 'ELIF MOBILYA-HAMIT ALTUN', 'MEHMET AKIF ERSOY MAH. GÜN CAD. NO:10/B', NULL, '792', '034', '3431', NULL, 0, 0, 'EMRAH POLAT', 'B', NULL),	 (1000000003541793, 1, 1, 9340782, '?RFAN BOZKURT', 'HAM?D?YE MAH HAM?D?YE CAD NO:57 / A 1', NULL, '792', '034', '3438', NULL, 0, 0, '?RFAN BOZKURT', 'B', NULL),	 (1000000003541794, 1, 1, 9340154, 'NURMEMET HANBABA', 'HALKALI MERKEZ MAH.ZEYNEB? YE CAD.NO:13 ', 'B', '792', '034', '3415', NULL, 0, 0, 'NURMEMET HANBABA', 'B', NULL),	 (1000000003541795, 1, 1, 56623, 'KELE?LER DTM- K?L?M MOB.', 'MERKEZ MAH.?SKELE CAD.NO:26', NULL, '792', '067', '6702', NULL, 0, 0, 'U?UR KELE?', 'B', NULL),	 (1000000003541796, 1, 1, 48392, 'YÜCEL T?CARET / YATA?', 'HAL?TPA?A MAH. MUSTAFA KEMAL PA?A CAD. ', 'NO:135', '792', '016', '1530', NULL, 0, 0, 'VEYSEL ÖZDEM?R', 'B', NULL),	 (1000000003541797, 1, 1, 9070087, 'ANTALYA DEMRE', 'GÖKYAZI MAH. ?LKOKUL 6. SOK. NO:11/C', 'DEMRE/ANTALYA', '792', '007', '0708', NULL, 0, 0, 'N?YAZ? DO?AN', 'B', NULL);
            "
, connection);
            var abi = pps.ExecuteNonQuery();

            SimpleTest(connection, "int", 10, 11, 12);
            SimpleTest(connection, "bigint", (long)10, (long)11, (long)12);
            SimpleTest(connection, "smallint", (short)10, (short)11, (short)12);
            SimpleTest(connection, "tinyint", (byte)10, (byte)11, (byte)12);
            SimpleTest(connection, "double", 10d, 11d, 12d);
            SimpleTest(connection, "float", 10d, 11d, 12d); // double is float !
            SimpleTest(connection, "varchar(255)", "10", "11", "12");
            SimpleTest(connection, "timestamp", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            SimpleTest(connection, "date", DateTime.Today, DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));
            //var x1 = table.ToXml();
            Console.WriteLine("Yuppiii , TEST OK.");
        }

        /// <summary>
        /// - Creates a table with one primary key
        /// - inserts a value
        /// - loads the table to a DataTable and checks it has the expected value
        /// - inserts another value, reloads and checks it's there
        /// - updates the previously inserted value, reloads and checks it has the expected updated value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="typeStr"></param>
        /// <param name="originalValue"></param>
        /// <param name="updatedValue"></param>
        /// <param name="insertedValue"></param>
        static void SimpleTest<T>(H2Connection connection, String typeStr, T originalValue, T updatedValue, T insertedValue)
        {
            var tn = "simple_test_" + typeStr.Replace('(', '_').Replace(')', '_');
            new H2Command("create table " + tn + " (value " + typeStr + " primary key)", connection).ExecuteNonQuery();
            dynamic val = originalValue;
            var originalValueStr =
                (originalValue is string) ? "'" + originalValue + "'" :
                (originalValue is DateTime) ? "parsedatetime('" + val.ToString("dd/MM/yyyy hh:mm:ss") + "', 'dd/MM/yyyy hh:mm:ss')" :
                    originalValue.ToString();
            new H2Command("insert into " + tn + " values (convert(" + originalValueStr + ", " + typeStr + "))", connection).ExecuteNonQuery();

            var adapter = new H2DataAdapter("select * from " + tn + " order by value", connection);
            new H2CommandBuilder(adapter);
            var table = new DataTable(tn)
            {
                CaseSensitive = false
            };
            adapter.Fill(table);

            Debug.Assert(table.Rows.Count.Equals(1));
            CheckVal(table.Rows[0][0], originalValue);

            table.Rows.Add(new object[] { insertedValue });
            adapter.Update(table);

            Reload(table, adapter);
            Debug.Assert(table.Rows.Count.Equals(2));
            CheckVal(table.Rows[1][0], insertedValue);

            table.Rows[1][0] = updatedValue;
            adapter.Update(table);

            Reload(table, adapter);
            Debug.Assert(table.Rows.Count.Equals(2));
            CheckVal(table.Rows[1][0], updatedValue);

        }
        static void Reload(DataTable table, H2DataAdapter adapter)
        {
            table.Clear();
            table.AcceptChanges();
            adapter.Fill(table);
        }
        static void CheckVal<T>(object value, T expectedVal)
        {
            Debug.Assert(value is T);
            var tvalue = (T)value;
            Debug.Assert(expectedVal.Equals(tvalue));
        }
    }

}
