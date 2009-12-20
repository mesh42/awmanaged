using System;
using System.Collections.Generic;
using AW;
using AwManaged.Interfaces;
using AwManaged.Math;

namespace AwManaged.SceneNodes
{
    public class WorldCloud  : IEngineReference
    {
        private readonly IBaseBotEngine engine;
        //AW_WORLD_CLOUDS_LAYER1_MASK	    String	 
        private string mask;
        //AW_WORLD_CLOUDS_LAYER1_OPACITY	Integer	 
        private int opacity;
        //AW_WORLD_CLOUDS_LAYER1_SPEED_X	Floating point	 
        private float speedX;
        //AW_WORLD_CLOUDS_LAYER1_SPEED_Z	Floating point	 
        private float speedZ;
        //AW_WORLD_CLOUDS_LAYER1_TEXTURE	String	 
        private string texture;
        //AW_WORLD_CLOUDS_LAYER1_TILE	    Floating point	 
        private float tile;

        public WorldCloud(IBaseBotEngine engine)
        {
            this.engine = engine;
        }

        #region IEngineReference Members

        public IBaseBotEngine  Engine
        {
            get { return engine; }
        }

        #endregion
    }

    public class WorldClouds : IEngineReference
    {
        private readonly IBaseBotEngine engine;

        public List<WorldCloud> Clouds { get; set;}

        public WorldClouds(IBaseBotEngine engine)
        {
            this.engine = engine;
            Clouds = new List<WorldCloud>(4);
        }

        #region IEngineReference Members

        public IBaseBotEngine  Engine
        {
            get { return engine; }
        }

        #endregion
    }

    public enum CellLimitType
    {
        Normal,Large,Huge,Mega,Ultra
    }

    public class WorldAttributes : IEngineReference
    {
        private IBaseBotEngine _engine;
        //AW_WORLD_ALLOW_3_AXIS_ROTATION	Boolean	 
        private bool allow3AxisRotation;
        //AW_WORLD_ALLOW_AVATAR_COLLISION	Boolean	 
        private bool allowAvatarCollision;
        //AW_WORLD_ALLOW_CITIZEN_WHISPER	Boolean	 
        private bool allowCitizenWhisper;
        //AW_WORLD_ALLOW_FLYING	Boolean	 
        private bool allowFlying;
        //AW_WORLD_ALLOW_OBJECT_SELECT	Boolean	 
        private bool allowObjectSelect;
        //AW_WORLD_ALLOW_PASSTHRU	Boolean	 
        private bool allowPassThru;
        //AW_WORLD_ALLOW_TELEPORT	Boolean	 
        private bool allowTeleport;
        //AW_WORLD_ALLOW_TOURIST_BUILD	Boolean	 
        private bool allowTouristBuild;
        //AW_WORLD_ALLOW_TOURIST_WHISPER	Boolean	 
        private bool allowTouristWhisper;
        //AW_WORLD_ALWAYS_SHOW_NAMES	Boolean	 
        private bool alwaysShowNames;
        //AW_WORLD_AMBIENT_LIGHT_BLUE	Integer	 
        private int worldAmbientLightBlue;
        //AW_WORLD_AMBIENT_LIGHT_GREEN	Integer	 
        private int worldAmbientLightGreen;
        //AW_WORLD_AMBIENT_LIGHT_RED	Integer	 
        private int worldAmbientlightRed;
        //AW_WORLD_AVATAR_REFRESH_RATE	Integer	 
        private int avatarRefreshRate;
        //AW_WORLD_BACKDROP	String	 
        private string backDrop;
        //AW_WORLD_BOTMENU_URL	String	 
        private string worldBotMenuUrl;
        //AW_WORLD_BOTS_RIGHT	String	 
        private string botsRight;
        //AW_WORLD_BUILD_CAPABILITY	Boolean	X
        private bool buildCapability;
        //AW_WORLD_BUILD_NUMBER	Integer	X
        private int buildNumber;
        //AW_WORLD_BUILD_RIGHT	String	 
        private string buildRight;
        //AW_WORLD_BUOYANCY	Floating point	 
        private float buoyancy;
        //AW_WORLD_CAMERA_ZOOM	Float	 
        private float cameraZoom;
        //AW_WORLD_CARETAKER_CAPABILITY	Boolean	X
        private bool caretakerCapability;
        //AW_WORLD_CAV_OBJECT_PASSWORD	String	 
        private bool cavObjectPassword;
        //AW_WORLD_CAV_OBJECT_PATH	String	 
        private string cavObjectPath;
        //AW_WORLD_CAV_OBJECT_REFRESH	Integer	 
        private int cavObjectRefresh;
        //AW_WORLD_CELL_LIMIT	Integer	 
        private CellLimitType cellLimit;
        //AW_WORLD_CHAT_DISABLE_URL_CLICKS	Boolean	 
        private bool chatDisableUrlClicks;
        //AW_WORLD_CLOUDS_LAYER1_MASK	String	 
        private string cloudsLayer1Mask;
        //AW_WORLD_CLOUDS_LAYER1_OPACITY	Integer	 
        private WorldClouds clouds;
        //AW_WORLD_CREATION_TIMESTAMP	Integer	X
        private DateTime creation;
        //AW_WORLD_DISABLE_AVATAR_LIST	Boolean	 
        private bool disableAvatarList;
        //AW_WORLD_DISABLE_CHAT	Boolean	 
        private bool disableChat;
        //AW_WORLD_DISABLE_CREATE_URL	Boolean	 
        private bool disableCreateUrl;
        //AW_WORLD_DISABLE_MULTIPLE_MEDIA	Boolean	 
        private bool disableMultipleMedia;
        //AW_WORLD_DISABLE_SHADOWS	Boolean	 
        private bool disableShadows;
        //AW_WORLD_EJECT_CAPABILITY	Boolean	X
        private bool ejectCapability;
        //AW_WORLD_EJECT_RIGHT	String	 
        private string ejectRight;
        //AW_WORLD_EMINENT_DOMAIN_CAPABILITY	Boolean	X
        private bool eminentDomainCapbility;
        //AW_WORLD_EMINENT_DOMAIN_RIGHT	String	 
        private string eminentDomainRight;
        //AW_WORLD_ENABLE_BUMP_EVENT	Boolean	 
        private bool enableBumpEvent;
        //AW_WORLD_ENABLE_CAMERA_COLLISION	Boolean	 
        private bool enableCameraCollision;        
        //AW_WORLD_ENABLE_CAV	Integer	 
        private int enableCav;
        //AW_WORLD_ENABLE_PAV	Boolean	 
        private bool enablePav;
        //AW_WORLD_ENABLE_REFERER	Boolean	 
        private bool enableReferer;
        //AW_WORLD_ENABLE_SYNC_EVENTS	Boolean	 
        private bool enableSyncEvents;
        //AW_WORLD_ENABLE_TERRAIN	Boolean	 
        private bool enableTerrain;
        //AW_WORLD_ENTER_RIGHT	String	 
        private string enterRight;
        //AW_WORLD_ENTRY_POINT	String	 
        private Vector3 entryPointPosition;
        private Vector3 entryPointRotation;
        //AW_WORLD_EXPIRATION	Integer	X
        private DateTime expiration;    
        //AW_WORLD_FOG_BLUE	Integer	 
        //AW_WORLD_FOG_GREEN	Integer	 
        //AW_WORLD_FOG_RED	Integer	 
        private Color fog;
        //AW_WORLD_FOG_ENABLE	Boolean	 
        private bool fogEnable;
        //AW_WORLD_FOG_MAXIMUM	Integer	 
        private int fogMaximum;
        //AW_WORLD_FOG_MINIMUM	Integer	 
        private int fogMinimum;
        //AW_WORLD_FOG_TINTED	Boolean	 
        private bool fogTinted;
        //AW_WORLD_FRICTION	Floating point	 
        private float friction;
        //AW_WORLD_GRAVITY	Floating point	 
        private float gravity;
        //AW_WORLD_GROUND	String	 
        private string ground;
        //AW_WORLD_HOME_PAGE	String	 
        private string homePage;
        //AW_WORLD_KEYWORDS	String	 
        private string keywords;
        //AW_WORLD_LIGHT_BLUE	Integer	 
        //AW_WORLD_LIGHT_GREEN	Integer	 
        //AW_WORLD_LIGHT_RED	Integer	 
        private Color light;
        //AW_WORLD_LIGHT_DRAW_BRIGHT	Boolean	 
        private bool lightDrawBright;
        //AW_WORLD_LIGHT_DRAW_FRONT	Boolean	 
        private bool lightDrawFront;
        //AW_WORLD_LIGHT_DRAW_SIZE	Integer	 
        private int lightDrawSize;
        //AW_WORLD_LIGHT_MASK	String

        //AW_WORLD_LIGHT_SOURCE_COLOR	Integer	 

        //AW_WORLD_LIGHT_SOURCE_USE_COLOR	Boolean	 

        //AW_WORLD_LIGHT_TEXTURE	String	 

        //AW_WORLD_LIGHT_X	Floating point	 

        //AW_WORLD_LIGHT_Y	Floating point	 

        //AW_WORLD_LIGHT_Z	Floating point	 

        //AW_WORLD_MAX_LIGHT_RADIUS	Integer	 

        //AW_WORLD_MAX_USERS	Integer	X

        //AW_WORLD_MINIMUM_VISIBILITY	Integer	 

        //AW_WORLD_MOVER_EMPTY_RESET_TIMEOUT	Integer	 

        //AW_WORLD_MOVER_USED_RESET_TIMEOUT	Integer	 

        //AW_WORLD_NAME	String	 

        //AW_WORLD_OBJECT_COUNT	Integer	X

        //AW_WORLD_OBJECT_PASSWORD	Data	 

        //AW_WORLD_OBJECT_PATH	String	 

        //AW_WORLD_OBJECT_REFRESH	Integer	 

        //AW_WORLD_PUBLIC_SPEAKER_CAPABILITY	Boolean	X

        //AW_WORLD_PUBLIC_SPEAKER_RIGHT	String	 

        //AW_WORLD_RATING	Integer	 

        //AW_WORLD_REPEATING_GROUND	Boolean	 

        //AW_WORLD_RESTRICTED_RADIUS	Integer	 

        //AW_WORLD_SIZE	Integer	X

        //AW_WORLD_SKY_BOTTOM_BLUE	Integer	 

        //AW_WORLD_SKY_BOTTOM_GREEN	Integer	 

        //AW_WORLD_SKY_BOTTOM_RED	Integer	 

        //AW_WORLD_SKY_EAST_BLUE	Integer	 

        //AW_WORLD_SKY_EAST_GREEN	Integer	 

        //AW_WORLD_SKY_EAST_RED	Integer	 

        //AW_WORLD_SKY_NORTH_BLUE	Integer	 

        //AW_WORLD_SKY_NORTH_GREEN	Integer	 

        //AW_WORLD_SKY_NORTH_RED	Integer	 

        //AW_WORLD_SKY_SOUTH_BLUE	Integer	 

        //AW_WORLD_SKY_SOUTH_GREEN	Integer	 

        //AW_WORLD_SKY_SOUTH_RED	Integer	 

        //AW_WORLD_SKY_TOP_BLUE	Integer	 

        //AW_WORLD_SKY_TOP_GREEN	Integer	 

        //AW_WORLD_SKY_TOP_RED	Integer	 

        //AW_WORLD_SKY_WEST_BLUE	Integer	 

        //AW_WORLD_SKY_WEST_GREEN	Integer	 

        //AW_WORLD_SKY_WEST_RED	Integer	 

        //AW_WORLD_SKYBOX	String	 

        //AW_WORLD_SLOPESLIDE_ENABLED	Boolean	 

        //AW_WORLD_SLOPESLIDE_MAX_ANGLE	Floating point	 

        //AW_WORLD_SLOPESLIDE_MIN_ANGLE	Floating point	 

        //AW_WORLD_SOUND_AMBIENT	String	 

        //AW_WORLD_SOUND_FOOTSTEP	String	 

        //AW_WORLD_SOUND_WATER_ENTER	String	 

        //AW_WORLD_SOUND_WATER_EXIT	String	 

        //AW_WORLD_SPEAK_CAPABILITY	Boolean	X

        //AW_WORLD_SPEAK_RIGHT	String	 

        //AW_WORLD_SPECIAL_COMMANDS	String	 

        //AW_WORLD_SPECIAL_COMMANDS_RIGHT	String	 

        //AW_WORLD_SPECIAL_OBJECTS_RIGHT	String	 

        //AW_WORLD_TERRAIN_AMBIENT	Floating point	 

        //AW_WORLD_TERRAIN_DIFFUSE	Floating point	 

        //AW_WORLD_TERRAIN_OFFSET	Floating point	 

        //AW_WORLD_TERRAIN_RIGHT	String	 

        //AW_WORLD_TERRAIN_TIMESTAMP	Integer	X

        //AW_WORLD_TITLE	String	 

        //AW_WORLD_V4_OBJECTS_RIGHT	String	 

        //AW_WORLD_WAIT_LIMIT	Integer	 

        //AW_WORLD_WATER_BLUE	Integer	 

        //AW_WORLD_WATER_BOTTOM_MASK	String	 

        //AW_WORLD_WATER_BOTTOM_TEXTURE	String	 

        //AW_WORLD_WATER_ENABLED	Boolean	 
//AW_WORLD_WATER_FRICTION	Floating point	 
//AW_WORLD_WATER_GREEN	Integer	 
//AW_WORLD_WATER_LEVEL	Floating point	 
//AW_WORLD_WATER_MASK	String	 
//AW_WORLD_WATER_OPACITY	Integer	 
//AW_WORLD_WATER_RED	Integer	 
//AW_WORLD_WATER_SPEED	Floating point	 
//AW_WORLD_WATER_SURFACE_MOVE	Floating point	 
//AW_WORLD_WATER_TEXTURE	String	 
//AW_WORLD_WATER_UNDER_TERRAIN	Boolean	 
//AW_WORLD_WATER_WAVE_MOVE	Floating point	 
//AW_WORLD_WATER_VISIBILITY	Integer	 
//AW_WORLD_WELCOME_MESSAGE	String	 
//AW_WORLD_VOIP_CONFERENCE_GLOBAL	Boolean	 
//AW_WORLD_VOIP_MODERATE_GLOBAL	Boolean	 
//AW_WORLD_VOIP_RIGHT	String	 



        public WorldAttributes(IBaseBotEngine engine) 
        {
            
        }

        #region IEngineReference Members

        public IBaseBotEngine Engine
        {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}