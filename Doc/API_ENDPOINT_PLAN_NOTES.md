# RaceDay API endpoint plan reference

The supplied API endpoint plan lists these planned groups:

- Authentication: `POST /api/auth/register`, `POST /api/auth/login`
- Profile/history: `GET/PUT /api/users/me`, `GET /api/users/me/history`
- Events: `GET /api/events`, `GET /api/events/{id}`, `POST/PUT/DELETE /api/events/{id}`
- Categories: event category listing and creation; category update/delete
- Routes: get/add/update route details per category
- Weather: get forecast per category
- Enrolments: enrol, list own enrolments, organiser event enrolments, cancel enrolment
- Results: capture/update results and list category results

The current project implements the main MVC browser workflows described in the root README. It does **not yet implement the full REST `/api/...` endpoint surface**, live weather integration, route editing, organiser enrolment management, or result capture/edit screens. These can be added as a next development phase.
