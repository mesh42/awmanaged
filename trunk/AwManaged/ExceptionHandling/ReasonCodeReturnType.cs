namespace AwManaged.ExceptionHandling
{
    /// <summary>
    /// Active World SDK Return Codes
    /// </summary>
    public enum ReasonCodeReturnType
    {
        /// <summary>
        /// Operation has completed successfully.
        /// </summary>
        [AwException(Message = "Operation has completed successfully.")]
        RC_SUCCESS = 0,
        /// <summary>
        /// Citizenship of the owner has expired.
        /// </summary>
        [AwException(Message = "Citizenship of the owner has expired.")]
        RC_CITIZENSHIP_EXPIRED = 1,
        /// <summary>
        /// Land limit of the universe would be exceeded if the world is started.
        /// </summary>
        [AwException(Message = "Land limit of the universe would be exceeded if the world is started.")]
        RC_LAND_LIMIT_EXCEEDED = 2,
        /// <summary>
        /// No citizenship with a matching citizen number was found.
        /// </summary>
        [AwException(Message = "No citizenship with a matching citizen number was found.")]
        RC_NO_SUCH_CITIZEN = 3,
        /// <summary>
        /// Password cannot contain a space.
        /// </summary>
        [AwException(Message = "Password cannot contain a space.")]
        RC_LICENSE_PASSWORD_CONTAINS_SPACE = 5,
        /// <summary>
        /// Password cannot be longer than 8 characters.
        /// </summary>
        [AwException(Message = "Password cannot be longer than 8 characters.")]
        RC_LICENSE_PASSWORD_TOO_LONG = 6,
        /// <summary>
        /// Password must be at least 2 characters.
        /// </summary>
        [AwException(Message = "Password must be at least 2 characters.")]
        RC_LICENSE_PASSWORD_TOO_SHORT = 7,
        /// <summary>
        /// Range must be smaller than 3275 hectometers. That is, at most 32750 coordinates N/S/W/E or 655000 meters across.
        /// </summary>
        [AwException(Message = "Range must be smaller than 3275 hectometers. That is, at most 32750 coordinates N/S/W/E or 655000 meters across.")]
        RC_LICENSE_RANGE_TOO_LARGE = 8,
        /// <summary>
        /// Range must be larger than 0 hectometers. That is, at least 10 coordinates N/S/W/E or 200 meters across.
        /// </summary>
        [AwException(Message = "Range must be larger than 0 hectometers. That is, at least 10 coordinates N/S/W/E or 200 meters across.")]
        RC_LICENSE_RANGE_TOO_SMALL = 9,
        /// <summary>
        /// User limit cannot exceed 1024.
        /// </summary>
        [AwException(Message = "User limit cannot exceed 1024.")]
        RC_LICENSE_USERS_TOO_LARGE = 10,
        /// <summary>
        /// User limit must be larger than 0.
        /// </summary>
        [AwException(Message = "User limit must be larger than 0.")]
        RC_LICENSE_USERS_TOO_SMALL = 11,
        /// <summary>
        /// Unable to login due to invalid password.
        /// </summary>
        [AwException(Message = "Unable to login due to invalid password.")]
        RC_INVALID_PASSWORD = 13,
        /// <summary>
        /// Name must be at least 2 characters.
        /// </summary>
        [AwException(Message = "Name must be at least 2 characters.")]
        RC_LICENSE_WORLD_TOO_SHORT = 15,
        /// <summary>
        /// Name cannot be longer than 8 characters.
        /// </summary>
        [AwException(Message = "Name cannot be longer than 8 characters.")]
        RC_LICENSE_WORLD_TOO_LONG = 16,
        /// <summary>
        /// Unable to start the world due to invalid name or password.
        /// </summary>
        [AwException(Message = "Unable to start the world due to invalid name or password.")]
        RC_INVALID_WORLD = 20,
        /// <summary>
        /// Server build either contains a serious flaw or is outdated and must be upgraded.
        /// </summary>
        [AwException(Message = "Server build either contains a serious flaw or is outdated and must be upgraded.")]
        RC_SERVER_OUTDATED = 21,
        /// <summary>
        /// World has already been started at a different location.
        /// </summary>
        [AwException(Message = "World has already been started at a different location.")]
        RC_WORLD_ALREADY_STARTED = 22,
        /// <summary>
        /// No world with a matching name has been started on the server.
        /// </summary>
        [AwException(Message = "No world with a matching name has been started on the server.")]
        RC_NO_SUCH_WORLD = 27,
        /// <summary>
        /// Not authorized to perform the operation.
        /// </summary>
        [AwException(Message = "Not authorized to perform the operation.")]
        RC_UNAUTHORIZED = 32,
        /// <summary>
        /// No license with a matching world name was found.
        /// </summary>
        [AwException(Message = "No license with a matching world name was found.")]
        RC_NO_SUCH_LICENSE = 34,
        /// <summary>
        /// Limit of started worlds in the universe would be exceeded if the world is started.
        /// </summary>
        [AwException(Message = "Limit of started worlds in the universe would be exceeded if the world is started.")]
        RC_TOO_MANY_WORLDS = 57,
        /// <summary>
        /// SDK build either contains a serious flaw or is outdated and must be upgraded.
        /// </summary>
        [AwException(Message = "SDK build either contains a serious flaw or is outdated and must be upgraded.")]
        RC_MUST_UPGRADE = 58,
        /// <summary>
        /// Bot limit of the owner citizenship would be exceeded if the bot is logged in.
        /// </summary>
        [AwException(Message = "Bot limit of the owner citizenship would be exceeded if the bot is logged in.")]
        RC_BOT_LIMIT_EXCEEDED = 59,
        /// <summary>
        /// Unable to start world due to its license having expired.
        /// </summary>
        [AwException(Message = "Unable to start world due to its license having expired.")]
        RC_WORLD_EXPIRED = 61,
        /// <summary>
        /// Citizen does not expire.
        /// </summary>
        [AwException(Message = "Citizen does not expire.")]
        RC_CITIZEN_DOES_NOT_EXPIRE = 62,
        /// <summary>
        /// Name cannot start with a number.
        /// </summary>
        [AwException(Message = "Name cannot start with a number.")]
        RC_LICENSE_STARTS_WITH_NUMBER = 64,
        /// <summary>
        /// No ejection with a matching identifier was found.
        /// </summary>
        [AwException(Message = "No ejection with a matching identifier was found.")]
        RC_NO_SUCH_EJECTION = 66,
        /// <summary>
        /// No user with a matching session number has entered the world.
        /// </summary>
        [AwException(Message = "No user with a matching session number has entered the world.")]
        RC_NO_SUCH_SESSION = 67,
        /// <summary>
        /// World has already been started.
        /// </summary>
        [AwException(Message = "World has already been started.")]
        RC_WORLD_RUNNING = 72,
        /// <summary>
        /// World to perform the operation on has not been set.
        /// </summary>
        [AwException(Message = "World to perform the operation on has not been set.")]
        RC_WORLD_NOT_SET = 73,
        /// <summary>
        /// No more cells left to enumerate.
        /// </summary>
        [AwException(Message = "No more cells left to enumerate.")]
        RC_NO_SUCH_CELL = 74,
        /// <summary>
        /// Unable to start world due to missing or invalid registry.
        /// </summary>
        [AwException(Message = "Unable to start world due to missing or invalid registry.")]
        RC_NO_REGISTRY = 75,
        /// <summary>
        /// Can't open registry.
        /// </summary>
        [AwException(Message = "Can't open registry.")]
        RC_CANT_OPEN_REGISTRY = 76,
        /// <summary>
        /// Citizenship of the owner has been disabled.
        /// </summary>
        [AwException(Message = " Citizenship of the owner has been disabled.")]
        RC_CITIZEN_DISABLED = 77,
        /// <summary>
        /// Unable to start world due to it being disabled.
        /// </summary>
        [AwException(Message = "Unable to start world due to it being disabled.")]
        RC_WORLD_DISABLED = 78,
        /// <summary>
        /// Telegram blocked.
        /// </summary>
        [AwException(Message = "Telegram blocked.")]
        RC_TELEGRAM_BLOCKED = 85,
        /// <summary>
        /// Unable to update terrain.
        /// </summary>
        [AwException(Message = "Unable to update terrain.")]
        RC_UNABLE_TO_UPDATE_TERRAIN = 88,
        /// <summary>
        /// Email address contains one or more invalid characters.
        /// </summary>
        [AwException(Message = "Email address contains one or more invalid characters.")]
        RC_EMAIL_CONTAINS_INVALID_CHAR = 100,
        /// <summary>
        /// Email address cannot end with a blank.
        /// </summary>
        [AwException(Message = "Email address cannot end with a blank.")]
        RC_EMAIL_ENDS_WITH_BLANK = 101,
        /// <summary>
        /// Unable to find the object to delete.
        /// </summary>
        [AwException(Message = "Unable to find the object to delete.")]
        RC_NO_SUCH_OBJECT = 101,
        /// <summary>
        /// Email address must contain at least one '.'.
        /// </summary>
        [AwException(Message = "Email address must contain at least one '.'.")]
        RC_EMAIL_MISSING_DOT = 102,
        /// <summary>
        /// Not delete owner.
        /// </summary>
        [AwException(Message = "Not delete owner.")]
        RC_NOT_DELETE_OWNER = 102,
        /// <summary>
        /// Email address must contain a '@'.
        /// </summary>
        [AwException(Message = "Email address must contain a '@'.")]
        RC_EMAIL_MISSING_AT = 103,
        /// <summary>
        /// Email address cannot start with a blank.
        /// </summary>
        [AwException(Message = "Email address cannot start with a blank.")]
        RC_EMAIL_STARTS_WITH_BLANK = 104,
        /// <summary>
        /// Email address cannot be longer than 50 characters.
        /// </summary>
        [AwException(Message = "Email address cannot be longer than 50 characters.")]
        RC_EMAIL_TOO_LONG = 105,
        /// <summary>
        /// Email address must be at least 8 characters or longer.
        /// </summary>
        [AwException(Message = "Email address must be at least 8 characters or longer.")]
        RC_EMAIL_TOO_SHORT = 106,
        /// <summary>
        /// Citizenship with a matching name already exists.
        /// </summary>
        [AwException(Message = "Citizenship with a matching name already exists.")]
        RC_NAME_ALREADY_USED = 107,
        /// <summary>
        /// Name contains invalid character(s).
        /// </summary>
        [AwException(Message = "Name contains invalid character(s).")]
        RC_NAME_CONTAINS_NONALPHANUMERIC_CHAR = 108,
        /// <summary>
        /// Name contains invalid blank(s).
        /// </summary>
        [AwException(Message = "Name contains invalid blank(s).")]
        RC_NAME_CONTAINS_INVALID_BLANK = 109,
        /// <summary>
        /// Name cannot end with a blank.
        /// </summary>
        [AwException(Message = "Name cannot end with a blank.")]
        RC_NAME_ENDS_WITH_BLANK = 111,
        /// <summary>
        /// Name cannot be longer than 16 characters.
        /// </summary>
        [AwException(Message = "Name cannot be longer than 16 characters.")]
        RC_NAME_TOO_LONG = 112,
        /// <summary>
        /// Name must be at least 2 characters.
        /// </summary>
        [AwException(Message = "Name must be at least 2 characters.")]
        RC_NAME_TOO_SHORT = 113,
        /// <summary>
        /// Password cannot be longer than 12 characters.
        /// </summary>
        [AwException(Message = "Password cannot be longer than 12 characters.")]
        RC_PASSWORD_TOO_LONG = 115,
        /// <summary>
        /// Password must be at least 4 characters.
        /// </summary>
        [AwException(Message = "Password must be at least 4 characters.")]
        RC_PASSWORD_TOO_SHORT = 116,
        /// <summary>
        /// Unable to delete citizen due to a database problem.
        /// </summary>
        [AwException(Message = "Unable to delete citizen due to a database problem.")]
        RC_UNABLE_TO_DELETE_CITIZEN = 124,
        /// <summary>
        /// Citizenship with a matching citizen number already exists.
        /// </summary>
        [AwException(Message = "Citizenship with a matching citizen number already exists.")]
        RC_NUMBER_ALREADY_USED = 126,
        /// <summary>
        /// Citizen number is larger than the auto-incremented field in the database.
        /// </summary>
        [AwException(Message = "Citizen number is larger than the auto-incremented field in the database.")]
        RC_NUMBER_OUT_OF_RANGE = 127,
        /// <summary>
        /// Privilege password must be either empty or at least 4 characters.
        /// </summary>
        [AwException(Message = "Privilege password must be either empty or at least 4 characters.")]
        RC_PRIVILEGE_PASSWORD_IS_TOO_SHORT = 128,
        /// <summary>
        /// Password cannot be longer than 12 characters.
        /// </summary>
        [AwException(Message = "Password cannot be longer than 12 characters.")]
        RC_PRIVILEGE_PASSWORD_IS_TOO_LONG = 129,
        /// <summary>
        /// Not permitted to change the owner of an object. It requires eminent domain or caretaker capability.
        /// </summary>
        [AwException(Message = "Not permitted to change the owner of an object. It requires eminent domain or caretaker capability.")]
        RC_NOT_CHANGE_OWNER = 203,
        /// <summary>
        /// Unable to find the object to change.
        /// </summary>
        [AwException(Message = "Unable to find the object to change.")]
        RC_CANT_FIND_OLD_ELEMENT = 204,
        /// <summary>
        /// Unable to enter world due to masquerading as someone else.
        /// </summary>
        [AwException(Message = "Unable to enter world due to masquerading as someone else.")]
        RC_IMPOSTER = 212,
        /// <summary>
        /// Not allowed to encroach into another's property.
        /// </summary>
        [AwException(Message = "Not allowed to encroach into another's property.")]
        RC_ENCROACHES = 300,
        /// <summary>
        /// Object type invalid.
        /// </summary>
        [AwException(Message = "Object type invalid.")]
        RC_OBJECT_TYPE_INVALID = 301,
        /// <summary>
        /// Cell limit would be exceeded.
        /// </summary>
        [AwException(Message = "Cell limit would be exceeded.")]
        RC_TOO_MANY_BYTES = 303,
        /// <summary>
        /// Model name does not exist in the registry.
        /// </summary>
        [AwException(Message = "Model name does not exist in the registry.")]
        RC_UNREGISTERED_OBJECT = 306,
        /// <summary>
        /// Element already exists.
        /// </summary>
        [AwException(Message = "Element already exists.")]
        RC_ELEMENT_ALREADY_EXISTS = 308,
        /// <summary>
        /// Restricted command.
        /// </summary>
        [AwException(Message = "Restricted command.")]
        RC_RESTRICTED_COMMAND = 309,
        /// <summary>
        /// Out of bounds.
        /// </summary>
        [AwException(Message = "Out of bounds.")]
        RC_OUT_OF_BOUNDS = 311,
        /// <summary>
        /// Not allowed to build with 'z' objects in this world.
        /// </summary>
        [AwException(Message = "Not allowed to build with 'z' objects in this world.")]
        RC_RESTRICTED_OBJECT = 313,
        /// <summary>
        /// Not allowed to build within the restricted area of this world.
        /// </summary>
        [AwException(Message = "Not allowed to build within the restricted area of this world.")]
        RC_RESTRICTED_AREA = 314,
        /// <summary>
        /// Would exceed the maximum number of operations per second.
        /// </summary>
        [AwException(Message = "Would exceed the maximum number of operations per second.")]
        RC_NOT_YET = 401,
        /// <summary>
        /// Synchronous operation timed out.
        /// </summary>
        [AwException(Message = "Synchronous operation timed out.")]
        RC_TIMEOUT = 402,
        /// <summary>
        /// Unable to establish a connection to the universe server.
        /// </summary>
        [AwException(Message = "Unable to establish a connection to the universe server.")]
        RC_UNABLE_TO_CONTACT_UNIVERSE = 404,
        /// <summary>
        /// Connection to the server is down.
        /// </summary>
        [AwException(Message = "Connection to the server is down.")]
        RC_NO_CONNECTION = 439,
        /// <summary>
        /// SDK API has not been initialized (by calling aw_init).
        /// </summary>
        [AwException(Message = "SDK API has not been initialized (by calling aw_init).")]
        RC_NOT_INITIALIZED = 444,
        /// <summary>
        /// No instance.
        /// </summary>
        [AwException(Message = "No instance.")]
        RC_NO_INSTANCE = 445,
        /// <summary>
        /// Invalid attribute.
        /// </summary>
        [AwException(Message = "Invalid attribute.")]
        RC_INVALID_ATTRIBUTE = 448,
        /// <summary>
        /// Type mismatch.
        /// </summary>
        [AwException(Message = "Type mismatch.")]
        RC_TYPE_MISMATCH = 449,
        /// <summary>
        /// String too long.
        /// </summary>
        [AwException(Message = "String too long.")]
        RC_STRING_TOO_LONG = 450,
        /// <summary>
        /// Unable to set attribute due to it being read-only.
        /// </summary>
        [AwException(Message = "Unable to set attribute due to it being read-only.")]
        RC_READ_ONLY = 451,
        /// <summary>
        /// Invalid instance.
        /// </summary>
        [AwException(Message = "Invalid instance.")]
        RC_INVALID_INSTANCE = 453,
        /// <summary>
        /// Aw.h and Aw.dll (or libaw_sdk.so for Linux) are from different builds of the SDK.
        /// </summary>
        [AwException(Message = "Aw.h and Aw.dll (or libaw_sdk.so for Linux) are from different builds of the SDK.")]
        RC_VERSION_MISMATCH = 454,
        /// <summary>
        /// A property query is already in progress.
        /// </summary>
        [AwException(Message = "A property query is already in progress.")]
        RC_QUERY_IN_PROGRESS = 464,
        /// <summary>
        /// Disconnected from world due to ejection.
        /// </summary>
        [AwException(Message = "Disconnected from world due to ejection.")]
        RC_EJECTED = 466,
        /// <summary>
        /// Citizenship of the owner does not have bot rights in the world.
        /// </summary>
        [AwException(Message = "Citizenship of the owner does not have bot rights in the world.")]
        RC_NOT_WELCOME = 467,
        /// <summary>
        /// Connection lost.
        /// </summary>
        [AwException(Message = "Connection lost.")]
        RC_CONNECTION_LOST = 471,
        /// <summary>
        /// Not available.
        /// </summary>
        [AwException(Message = "Not available.")]
        RC_NOT_AVAILABLE = 474,
        /// <summary>
        /// Can't resolve universe host.
        /// </summary>
        [AwException(Message = "Can't resolve universe host.")]
        RC_CANT_RESOLVE_UNIVERSE_HOST = 500,
        /// <summary>
        /// Invalid argument.
        /// </summary>
        [AwException(Message = "Invalid argument.")]
        RC_INVALID_ARGUMENT = 505,
        /// <summary>
        /// Unable to update custom avatar.
        /// </summary>
        [AwException(Message = " Unable to update custom avatar.")]
        RC_UNABLE_TO_UPDATE_CAV = 514,
        /// <summary>
        /// Unable to delete custom avatar.
        /// </summary>
        [AwException(Message = "Unable to delete custom avatar.")]
        RC_UNABLE_TO_DELETE_CAV = 515,
        /// <summary>
        /// No such custom avatar.
        /// </summary>
        [AwException(Message = "No such custom avatar.")]
        RC_NO_SUCH_CAV = 516,
        /// <summary>
        /// World instance already exists.
        /// </summary>
        [AwException(Message = "World instance already exists.")]
        RC_WORLD_INSTANCE_ALREADY_EXISTS = 521,
        /// <summary>
        /// World instance invalid.
        /// </summary>
        [AwException(Message = "World instance invalid.")]
        RC_WORLD_INSTANCE_INVALID = 522,
        /// <summary>
        /// Plugin not available.
        /// </summary>
        [AwException(Message = "Plugin not available.")]
        RC_PLUGIN_NOT_AVAILABLE = 523,
        /// <summary>
        /// Database error.
        /// </summary>
        [AwException(Message = "Database error.")]
        RC_DATABASE_ERROR = 600,
        /// <summary>
        /// Not enough room in the output buffer.
        /// </summary>
        [AwException(Message = "Not enough room in the output buffer.")]
        RC_Z_BUF_ERROR = 4995,
        /// <summary>
        /// Memory could not be allocated for processing.
        /// </summary>
        [AwException(Message = "Memory could not be allocated for processing.")]
        RC_Z_MEM_ERROR = 4996,
        /// <summary>
        /// Input data was corrupted.
        /// </summary>
        [AwException(Message = "Input data was corrupted.")]
        RC_Z_DATA_ERROR = 4997
    }
}