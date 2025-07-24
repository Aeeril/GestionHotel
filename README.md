
## GestionHotel – Application de gestion hôtelière

Application ASP.NET Core modulaire permettant la gestion complète d’un hôtel : réservations, chambres, utilisateurs, paiements, notifications par email, et signalements de casse.

---

### Structure du projet

```
GestionHotel-Projet-Final/
│
├── GestionHotel.Domain/              # Entités métiers et interfaces
├── GestionHotel.Application/         # Services métiers et background jobs
├── GestionHotel.Infrastructure/      # Repositories, DB context (EF Core)
├── GestionHotel.Externals.PaiementGateways/  # Services externes (mock de paiements)
└── GestionHotel.Apis/                # API REST avec Swagger
```

---

### Technologies

* ASP.NET Core 8
* Entity Framework Core + SQLite
* Architecture hexagonale (Ports & Adapters)
* Swagger UI
* Mailtrap (en dev) pour les notifications mail
* Hosted Services pour les notifications pré/post-séjour

---

### Lancer le projet

#### 1. Prérequis

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
* (Optionnel) [Mailtrap.io](https://mailtrap.io) pour simuler l’envoi de mails

#### 2. Build & Run

```bash
dotnet build
dotnet run --project GestionHotel.Apis
```

L’API sera disponible sur :
 `http://localhost:5123/swagger`

---

### Notifications automatiques

* **Notification pré-séjour** : 1 jour avant la date de début
* **Notification post-séjour** : 1 jour après la date de fin
* Services exécutés via `BackgroundService`

Mailtrap limite à 50 emails par mois pour les comptes gratuits.

---

## Fonctionnalités principales

### 📦 Gestion des Chambres

| Méthode | Endpoint                              | Description                          |
| ------: | ------------------------------------- | ------------------------------------ |
|     GET | `/api/Chambres`                       | Liste de toutes les chambres         |
|     GET | `/api/Chambres/disponibles`           | Chambres disponibles                 |
|     GET | `/api/Chambres/disponibles/reception` | Chambres disponibles (vue réception) |
|    POST | `/api/Chambres`                       | Créer une chambre                    |
|     PUT | `/api/Chambres/{id}`                  | Modifier une chambre                 |
|  DELETE | `/api/Chambres/{id}`                  | Supprimer une chambre                |

---

### 👥 Gestion des Clients

| Méthode | Endpoint            | Description         |
| ------: | ------------------- | ------------------- |
|     GET | `/api/Clients`      | Liste des clients   |
|    POST | `/api/Clients`      | Ajouter un client   |
|     PUT | `/api/Clients/{id}` | Modifier un client  |
|  DELETE | `/api/Clients/{id}` | Supprimer un client |

---

### 📅 Réservations

| Méthode | Endpoint                                  | Description                     |
| ------: | ----------------------------------------- | ------------------------------- |
|     GET | `/api/Reservations/actives`               | Réservations actives            |
|    POST | `/api/Reservations`                       | Créer une réservation           |
|    POST | `/api/Reservations/checkin`               | Check-in d’un client            |
|    POST | `/api/Reservations/checkout`              | Check-out d’un client           |
|    POST | `/api/Reservations/annuler/{id}`          | Annuler une réservation         |
|    POST | `/api/Reservations/annuler-par-reception` | Annulation rapide via réception |

---

### 🛠️ Signalements de casse

| Méthode | Endpoint                 | Description                         |
| ------: | ------------------------ | ----------------------------------- |
|     GET | `/api/Signalements`      | Liste des signalements              |
|    POST | `/api/Signalements`      | Signaler une casse                  |
|     PUT | `/api/Signalements/{id}` | Marquer un signalement comme traité |
|  DELETE | `/api/Signalements/{id}` | Supprimer un signalement            |

---

### Notifications par e-mail

Services de notifications automatiques exécutés en tâche de fond (`BackgroundService`) :

* **Pré-séjour** : envoi d’un rappel au client 1 jour avant le début du séjour.
* **Post-séjour** : remerciement au client 1 jour après la fin du séjour.

ℹ️ Envoi limité dans Mailtrap à 50 mails/mois sur la version gratuite.

---

## Tests

Swagger pour tester tous les endpoints manuellement à l’adresse :
    `http://localhost:5123/swagger`

---

### Bonnes pratiques suivies

* Injection de dépendances
* Séparation claire des responsabilités
* Logs structurés via `ILogger`
* Sécurité EF Core : usage d'`async`, contrôle des exceptions

---

### 🧑‍💻 Auteur

Benjamin PRUJA, Nicolas FIACSAN – 2025
Projet de cours : **Architecture Applicative**