### **Smart Healthcare System (Web API Project)**

#### **Overview**  
A robust Web API designed for managing healthcare-related data including users, reviews, nurses, doctors, medical centers, diseases, departments, bookings, and awareness videos. The system provides efficient ways to manage healthcare operations and ensure security through JWT-based authentication.

---

#### **Key Features**
- **Authentication & Authorization:**  
  Secure user authentication with JWT tokens, ensuring safe access to sensitive endpoints.

- **User Management:**  
  Create, update, delete, and fetch user information including support for registration, login, and password reset functionalities.

- **Healthcare Professional Management:**  
  Manage doctors, nurses, and their profiles, including appointment bookings for both.

- **Medical Center & Department Management:**  
  Handle medical center and department creation, and retrieval of related information.

- **Review System:**  
  Users can add reviews for healthcare services, fostering communication and feedback within the system.

- **Disease and Symptom Management:**  
  Create and track diseases and symptoms, and link them for a comprehensive medical dataset.

- **Awareness Videos:**  
  Create and manage awareness videos related to healthcare topics, providing educational resources.

- **Booking System:**  
  Manage appointments for doctors and nurses, including booking creation and retrieval.

- **API Documentation:**  
  Detailed Swagger documentation for easy interaction and testing of all API endpoints.

---

#### **Technologies**
- **Backend:** ASP.NET Core Web API, Entity Framework Core  
- **Authentication:** JWT (JSON Web Token)  
- **Database:** SQL Server  
- **Serialization:** System.Text.Json  
- **Documentation:** Swagger/OpenAPI  

---

#### **Endpoints Overview**

##### **User Management**
- `POST: api/Users/register` – Register a new user.  
- `POST: api/Users/login` – Authenticate and return JWT token.  
- `POST: api/Users/SendTokenToEmail` – Send token to user email.  
- `POST: api/Users/ResetPassword` – Reset the user password.  
- `GET: api/Users/GetAll` – Retrieve all users.  
- `GET: api/Users/{id}` – Retrieve user details by ID.  

##### **Review Management**
- `GET: api/Reviews/GetAll` – Retrieve all reviews.  
- `GET: api/Reviews/{id}` – Retrieve a review by ID.  
- `POST: api/Reviews/Create` – Add a new review.  

##### **Healthcare Professional Management**
- `GET: api/Nurses/GetAll` – Retrieve all nurses.  
- `GET: api/Nurses/{id}` – Retrieve nurse details by ID.  
- `POST: api/Nurses/Create` – Add a new nurse.  

- `GET: api/Doctors/GetAll` – Retrieve all doctors.  
- `GET: api/Doctors/{id}` – Retrieve doctor details by ID.  
- `POST: api/Doctors/Create` – Add a new doctor.  

##### **Medical Center & Department Management**
- `GET: api/MedicalCenters/GetAll` – Retrieve all medical centers.  
- `GET: api/MedicalCenters/{id}` – Retrieve medical center details by ID.  
- `POST: api/MedicalCenters/Create` – Add a new medical center.  

- `GET: api/Departments/GetAll` – Retrieve all departments.  
- `GET: api/Departments/{id}` – Retrieve department details by ID.  
- `POST: api/Departments/Create` – Add a new department.  

##### **Disease and Symptom Management**
- `POST: api/Diseases/CreateDisease` – Create a new disease.  
- `POST: api/Diseases/CreateSymptom` – Create a new symptom.  
- `POST: api/Diseases/CreateDiseaseSymptom` – Link disease and symptom.  
- `GET: api/Diseases/GetAllDisease` – Retrieve all diseases.  

##### **Booking Management**
- `GET: api/Bookings/GetAllBookingsForDoctor` – Retrieve all bookings for a doctor.  
- `POST: api/Bookings/CreateBookingForDoctor` – Create a new booking for a doctor.  
- `GET: api/Bookings/GetAllBookingsForNurse` – Retrieve all bookings for a nurse.  
- `POST: api/Bookings/CreateBookingForNurse` – Create a new booking for a nurse.  

##### **Awareness Videos Management**
- `POST: api/AwarenessVideos/CreateAwarenessVideo` – Create a new awareness video.  
- `POST: api/AwarenessVideos/CreateUsersVideos` – Create a new video for users.  
- `GET: api/AwarenessVideos/GetAllVideos` – Retrieve all awareness videos.  

---

#### **Security & Validation**
- **Authentication:**  
  Secure JWT-based token authentication for all endpoints.  

- **Authorization:**  
  Ensures that only authenticated users can access certain endpoints.

- **Validation:**  
  Ensures input data is validated for accuracy and integrity.

- **Error Handling:**  
  Provides descriptive error messages to help identify issues during API usage.  

---

#### **Outcome**
Delivered a secure and scalable Web API designed to manage various healthcare operations, from user management to booking, reviews, and awareness content, with a focus on security, performance, and usability.

---

## How to Use API Collection:

1. **Download the desired collection** from the links above.
2. Open Postman.
3. Click on the "Import" button in the top left corner.
4. Select the downloaded JSON file and import it.
5. Start testing the API endpoints.

---

## Notes:
- Ensure you have the correct authentication tokens to access the endpoints.
