# clinic-assessment-dotnet
 Clinic project assessment

*** Some additional tasks have been included in the BONUS folder ***

 Port = 8090
 Prefix to endpoint = /api
 ________________________________________________________________________________________________________________________________________
|  Endpoint                             |  Mapping  | Requirement                                                 | Access               |
| ------------------------------------- | --------- | ----------------------------------------------------------- | -------------------- |
| /register                             | POST       | Allow all users to register                                 | All                  |
| /login                                | POST      | Allow users to login                                        | All                  |
| /doctors                              | GET       | View list of doctors                                        | All                  |
| /doctor/{id}                         | GET       | View Doctor information                                     | All                  |
| /doctor/{id}/slots?date=             | GET       | View Doctors available slots                                | Doctor, Patient, CA  |
| /appointment/book                     | POST       | Book an appointment with a doctor                           | Patient              |
| /appointment/cancel                   | PUT      | Cancel appointment                                          | Doctor, CA           |
| /doctor/all/slots?date=              | GET       | View availability of all Doctors                            | Doctor, Patient, CA  |
| /appointment/{id}/details             | GET       | View appointment details                                    | Doctor, Patient      |
| /appointment/all?patient_id=          | GET       | View patient appointment history                            | Doctor, Patient      |
| /doctor/busy/{date}                  | GET       | View doctors with the most appointments in a given day      | CA                   |
| /doctor/busy/{date}/{minimum-hours}  | GET       | View doctors who have 6+ hours total appointments in a day  | CA                   |
 ----------------------------------------------------------------------------------------------------------------------------------------
 
 
