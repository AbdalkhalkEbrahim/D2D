API Endpoints Documentation

This document lists controllers, their actions, input parameters (field type and nullability), and possible return results (HTTP status codes and error messages produced by application handlers).

Note: Error status codes map from application-level Error.StatusCode via BaseApiController.HandleFailure.

Controllers

1) AuthController (Route: /api/auth)

- POST /api/auth/register
  - Action: Register
  - Input (UserRegisterationCommand) [FromBody]
	- FirstName: string (required)
	- LastName: string (required)
	- Email: string (required, email)
	- Password: string (required)
	- ComfirmedPassword: string (required)
	- UserType: enum UserType (required)
	- Day: int (required)
	- Month: int (required)
	- Year: int (required)
  - Produces: 200 OK (UserRegisterationResponse), 400 Bad Request, 409 Conflict, 500 Internal Server Error
  - Handler failures (examples): BadRequest (PasswordMismatch, Underage, ValidationError), Conflict (Email), SystemError (UserCreationFailed)

- POST /api/auth/send-otp
  - Action: SendOtp
  - Input (SendOtpCommand) [FromBody]
	- ID: string (required)
	- Email: string (string, optional)
	- flag: bool (optional, default true)
  - Produces: 200 OK (OtpResponse), 404 Not Found, 403 Forbidden, 500 Internal Server Error
  - Handler failures: NotFound (User), Conflict (Email), Forbidden (OtpBackoff), SystemError

- POST /api/auth/verify-otp
  - Action: VerifyOtp
  - Input (VerifyOtpCommand) [FromBody]
	- UserId: string (required)
	- Otp: string (required, regex 6 chars)
	- flag: bool (optional, default true)
  - Produces: 200 OK (OtpResponse), 400 Bad Request, 404 Not Found, 410 Gone
  - Handler failures: NotFound (User), Expired (Otp)

- POST /api/auth/customer-registeration
  - Action: CustomerRegisteration
  - Input (CustomerRegisterationCommand) [FromForm]
	- CustomerId: string (required)
	- FrontImageID: IFormFile (required)
	- BackImageID: IFormFile (required)
	- PersonalImage: IFormFile (required)
	- AppartmentNo: string (required)
	- BuildingNumber: int (required)
	- Street: string (required)
	- District: string (required)
	- City: string (required)
	- Goverate: string (required)
  - Produces: 200 OK (CustomerRegisteratonResponse), 400 Bad Request, 500 Internal Server Error
  - Handler failures: BadRequest (InvalidRequest), SystemError

- POST /api/auth/producer-registeration
  - Action: ProducerRegisteration
  - Input (ProducerRegisterrationCommand) [FromForm]
	- ProducerId: string (required)
	- FrontImageID: IFormFile (required)
	- BackImageID: IFormFile (required)
	- PersonalImage: IFormFile (required)
	- LicenseUrls: List<IFormFile> (required, min length 1)
  - Produces: 200 OK (ProducerRegisterationResponse), 400 Bad Request, 500 Internal Server Error
  - Handler failures: BadRequest (InvalidRequest), SystemError

- POST /api/auth/designer-registeration
  - Action: DesignerRegisteration
  - Input (DesignerRegesterationCommand) [FromForm]
	- DesignerId: string (required)
	- FrontImageID: IFormFile (required)
	- BackImageID: IFormFile (required)
	- PersonalImage: IFormFile (required)
	- StepUrls: List<IFormFile> (required, min length 3)
  - Produces: 200 OK (DesignerRegisterationResponse), 400 Bad Request, 500 Internal Server Error
  - Handler failures: BadRequest (InvalidRequest), SystemError

- POST /api/auth/login
  - Action: Login
  - Input (UserLoginCommand) [FromBody]
	- Email: string (required, email)
	- Password: string (required)
  - Produces: 200 OK (object with JWT), 400 Bad Request, 404 Not Found, 403 Forbidden, 423 Locked
  - Handler failures: BadRequest (InvalidCredentials), AccountStatus (Rejected, Pending), AccountLocked (423)

- POST /api/auth/google-login
  - Action: GoogleLogin
  - Input (GoogleLoginCommand) [FromBody]
	- IdToken: string (required)
  - Produces: 200 OK (JwtToken), 400 Bad Request, 404 Not Found
  - Handler failures: BadRequest (InvalidCredentials), NotFound (User)

- POST /api/auth/forget-password
  - Action: ForgetPassword
  - Input (ForgetPasswordCommand) [FromBody]
	- Id: string (required)
	- Email: string (required, email)
	- Otp: string (required, regex 6 chars)
	- NewPassword: string (required)
	- ConfirmPassword: string (required)
  - Produces: 200 OK (string), 400 Bad Request, 404 Not Found, 410 Gone
  - Handler failures: BadRequest (PasswordMismatch, PasswordChangeFailed), NotFound (User)

- POST /api/auth/refresh-token
  - Action: RefreshToken
  - Input: refreshToken: string (from body or query)
  - Produces: 200 OK (JwtToken), 400 Bad Request, 410 Gone, 404 Not Found
  - Handler failures: Expired (Token), NotFound

- POST /api/auth/signout
  - Action: SignOut
  - Input: refreshToken: string (FromBody)
  - Produces: 204 No Content (bool), 400 Bad Request
  - Handler failures: BadRequest

- POST /api/auth/send-login-link
  - Action: SendLoginLink
  - Input (SendLoginLinkCommand) [FromBody]
	- UserID: string (required)
  - Produces: 200 OK (string), 404 Not Found, 500 Internal Server Error
  - Handler failures: NotFound (User), SystemError

- GET /api/auth/verify-magic-token
  - Action: VerifyMagicToken
  - Input (VerifyMagicTokenCommand) [FromBody]
	- Token: string
  - Produces: 200 OK (JwtToken), 400 Bad Request, 404 Not Found, 410 Gone
  - Handler failures: Expired (MagicToken), NotFound (User)


2) AccountSettingsController (Route: /api/accountsettings)

- POST /api/accountsettings/change-password
  - Action: ChangePassword
  - Input (ChangePasswordCommand) [FromBody]
	- UserId: string (required)
	- CurrentPassword: string (required)
	- NewPassword: string (required)
	- ConfirmPassword: string (required)
  - Produces: 200 OK (string), 400 Bad Request, 404 Not Found
  - Handler failures: BadRequest (PasswordChangeFailed, PasswordMismatch), NotFound (User)

- POST /api/accountsettings/add-new-address
  - Action: AddNewAddress
  - Input (AddNewAddressCommand) [FromBody]
	- CustomerId: string (required)
	- AppartmentNo: string (required)
	- BuildingNumber: int (required)
	- Street: string (required)
	- District: string (required)
	- City: string (required)
	- Goverate: string (required)
	- Selected: bool (optional, default false)
  - Produces: 200 OK (int created address id), 409 Conflict, 404 Not Found
  - Handler failures: NotFound (User)

- GET /api/accountsettings/get-address/{id}
  - Action: GetAddressById
  - Input: id: int (route)
  - Produces: 200 OK (AddressResponse), 404 Not Found
  - Handler failures: NotFound (Address)

- GET /api/accountsettings/get-customer-addresses/{customerId}
  - Action: GetCustomerAddresses
  - Input: customerId: string (route)
  - Produces: 200 OK (List<AddressResponse>), 404 Not Found
  - Handler failures: NotFound (Address/User)

- DELETE /api/accountsettings/delete-address/{id}
  - Action: DeleteAddress
  - Input: id: int (route)
  - Produces: 200 OK, 404 Not Found, 409 Conflict (returns 204 on success)
  - Handler failures: NotFound (User/Address), Conflict (Address)

- GET /api/accountsettings/get-customer-profile/{customerId}
  - Action: GetCustomerProfile
  - Input: customerId: string (route)
  - Produces: 200 OK (CustomerProfileResponse), 404 Not Found
  - Handler failures: NotFound (User)

- GET /api/accountsettings/get-profile/{userId}
  - Action: GetProfile
  - Input: userId: string (route)
  - Produces: 200 OK (ProfileResponse), 404 Not Found
  - Handler failures: NotFound (User)

- POST /api/accountsettings/edit-profile
  - Action: EditProfile
  - Input (EditProfileCommand) [FromForm]
	- UserId: string (required)
	- FirstName: string? (optional)
	- LastName: string? (optional)
	- ProfileImageUrl: IFormFile? (optional)
	- PhoneNumber: string? (optional)
  - Produces: 200 OK (ProfileResponse), 404 Not Found
  - Handler failures: NotFound (User)

- POST /api/accountsettings/change-email
  - Action: ChangeEmail
  - Input (ChangeEmailCommand) [FromBody]
	- Id: string
	- Otp: string
	- Email: string (email)
  - Produces: 200 OK (string), 404 Not Found, 410 Gone
  - Handler failures: NotFound (User), Expired (Otp)


3) ChatController (Route: /api/chat)

- POST /api/chat/customer-send-message
  - Action: CustomerSendMessage
  - Input (CustomerSendMessageCommand) [FromBody]
	- ChatId: int
	- MessageText: string
  - Produces: 204 No Content, 404 Not Found, 403 Forbidden
  - Handler failures: NotFound (Chat), Forbidden (ChatLimit)

- POST /api/chat/producer-send-message
  - Action: ProducerSendMessage
  - Input (ProducerSendMessageCommand) [FromBody]
	- ChatId: int
	- MessageText: string? (optional)
	- MessageImageUrl: IFormFile? (optional)
  - Produces: 204 No Content, 404 Not Found, 403 Forbidden, 400 Bad Request
  - Handler failures: BadRequest (NullValue), NotFound (Chat), Forbidden (ChatLimit)

- POST /api/chat/change-active-offer-status
  - Action: ChangeActiveOfferStatus
  - Input (ChangeActiveOfferStatusCommand) [FromBody]
	- ChatID: int
	- Status: ActiveOfferStatus (enum)
  - Produces: 200 OK (string), 404 Not Found
  - Handler failures: NotFound (Default)

- GET /api/chat/get-all-chats
  - Action: GetAllChats
  - Input (GetAllChatsQuery) [FromQuery]
	- UserId: string
	- Type: UserType (enum)
	- Unread: bool (optional, default false)
	- Completed: bool (optional, default false)
	- OnHold: bool (optional, default false)
  - Produces: 200 OK (List<ChatsResponse>), 404 Not Found
  - Handler failures: NotFound (Chat)


4) DesignController (Route: /api/design)

- GET /api/design/get-design/{Id}
  - Action: GetDesign
  - Input: Id: Guid (route)
  - Produces: 200 OK (Guid/DesignResponse), 404 Not Found
  - Handler failures: NotFound (Design)

- POST /api/design/save-design
  - Action: SvaeDesign
  - Input (SaveDesignCommand) [FromForm]
	- Id: string
	- Name: string (required, max 50)
	- DesignImage: IFormFile (optional)
  - Produces: 200 OK (Guid), 404 Not Found
  - Handler failures: NotFound (User), Conflict (Design)

- DELETE /api/design/delete-design/{Id}
  - Action: DeleteDesign
  - Input: Id: Guid (route)
  - Produces: 204 No Content, 400 Bad Request
  - Handler failures: BadRequest (Design)

- GET /api/design/get-customer-designs
  - Action: GetCustomerDesigns
  - Input (GetAllDesignsQuery) [FromQuery]
	- CustomerId: string
	- Name: string? (optional)
	- StartDate: DateTime? (optional)
	- EndDate: DateTime? (optional)
	- Status: DesignStatus? (optional)
	- PageSize: int? (optional, default 10)
	- PageNum: int? (optional, default 1)
  - Produces: 200 OK (List<DesignResponse>), 404 Not Found
  - Handler failures: NotFound (User/Design)

- POST /api/design/published-to-drafted
  - Action: PublishedToDrafted
  - Input (PublishedToDraftedCommand) [FromBody]
	- DesignId: Guid
  - Produces: 200 OK (Guid), 404 Not Found
  - Handler failures: BadRequest (DraftedDesign)


5) OfferController (Route: /api/offer)

- POST /api/offer/customer-publish-design
  - Action: CustomerPublishOffer
  - Input (CustomerPublishOfferCommand) [FromBody]
	- DesignId: Guid
	- Name: string
	- Category: string
	- Description: string
	- TargetAudience: string
	- Gender: bool
	- Colors: List<string>
	- Material: string? (optional)
	- PrintingType: string? (optional)
	- Sizes: List<string>
	- SizesFile: IFormFile? (optional)
	- Duration: int
	- Amount: int
	- MaxPrice: decimal
  - Produces: 201 Created (Guid), 404 Not Found, 410 Gone
  - Handler failures: NotFound (Design), Conflict (Offer)

- POST /api/offer/producer-customer-offer
  - Action: ProducerCustomerOffer
  - Input (ProducerCustomerOfferCommand) [FromBody]
	- CustomerId: Guid
	- ProducerId: string
	- CustomerPublishedOfferId: Guid
	- Price: decimal
  - Produces: 409 Conflict
  - Handler failures: NotFound (User), Conflict (CustomerOffer)

- GET /api/offer/get-published-design-details
  - Action: GetPublishedDesignDetails
  - Input (GetPublishedDesignByIdQuery) [FromQuery]
	- OfferId: Guid
  - Produces: 200 OK (CustomerOfferResponse), 404 Not Found
  - Handler failures: NotFound (Offer)

- GET /api/offer/get-all-published-designs
  - Action: GetAllPublishedDesigns
  - Input (GetAllPublishedesignsQuery) [FromQuery]
	- Gender: int
	- Category: string? (optional)
	- Duration: int
	- Amount: int
	- MaxPrice: int
	- CreationOrder: int
	- PageSize: int? (optional)
	- PageNum: int? (optional)
  - Produces: 200 OK (List<CustomerOfferResponse>), 404 Not Found
  - Handler failures: NotFound (Design)

- PATCH /api/offer/edit-published-design
  - Action: EditPublishedDesign
  - Input (EditPublishedDesignCommand) [FromBody]
	- DesignId: Guid
	- data: JsonPatchDocument<PublishedDesign>
  - Produces: 200 OK (Guid), 404 Not Found
  - Handler failures: NotFound (Design)

- POST /api/offer/edit-producer-customer-offer
  - Action: EditProducerCustomerOffer
  - Input (EditProducerCustomerOfferCommand) [FromBody]
	- OfferId: Guid
	- ProducerId: string
	- Price: decimal
  - Produces: 200 OK (Guid), 404 Not Found
  - Handler failures: NotFound (Offer)

- GET /api/offer/get-producer-offer-details
  - Action: GetProducerOfferDetails
  - Input (GetProducerCustomerOfferByIdQuery) [FromQuery]
	- OfferId: Guid
  - Produces: 200 OK (ProducerOfferResponse), 404 Not Found
  - Handler failures: NotFound (Offer)

- GET /api/offer/get-Published-design-offers
  - Action: GetAllProducerOffers
  - Input (GetOffersOnPublishedDesignQuery) [FromQuery]
	- PublishedDesignId: Guid
	- LowestPrice: bool
	- NewestOffer: bool
	- BestMatch: bool
	- HighestRate: bool
	- PageSize: int? (optional)
	- PageNum: int? (optional)
  - Produces: 200 OK (List<ProducerOfferResponse>), 404 Not Found
  - Handler failures: NotFound (Offer)

- POST /api/offer/decline-producer-offer
  - Action: DeclineProducerOffer
  - Input (DeclineProducerOfferCommand) [FromBody]
	- OfferId: Guid
	- ProducerId: Guid
  - Produces: 200 OK (Guid), 404 Not Found
  - Handler failures: NotFound (Offer)


Notes and how to generate PDF:
- File created: Documentation/API-Endpoints.md
- To convert to PDF locally, use pandoc or another Markdown-to-PDF tool. Example (PowerShell):
  pandoc Documentation\API-Endpoints.md -o Documentation\API-Endpoints.pdf

