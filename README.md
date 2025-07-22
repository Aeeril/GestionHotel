# 🏨 Système de Gestion d'Hôtel – API ASP.NET Core

## 📦 Présentation
Ce projet est une API web pour la gestion d’un hôtel. Elle permet :
- la gestion des réservations,
- la consultation de la disponibilité des chambres,
- la gestion des utilisateurs et du personnel,
- la simulation de paiements.

> Architecture basée sur Clean Architecture (Domain, Application, Infrastructure, API)

---

## 🚀 Lancement du projet

### 1. Prérequis
- .NET 8 SDK
- Visual Studio 2022 ou VS Code

### 2. Migration et Base de Données
```bash
cd GestionHotel.Apis
dotnet ef migrations add InitialCreate --project ../GestionHotel.Infrastructure --startup-project . --context HotelDbContext
dotnet ef database update
```

Cela crée automatiquement le fichier `hotel.db` (SQLite).

### 3. Lancer l'application
```bash
dotnet run
```

Accès à Swagger : [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## 🔐 Authentification
> Une version simple avec rôles (`Client`, `Receptionniste`, `PersonnelMenage`) est prévue via email/mot de passe.

---

## 📚 Structure
- `GestionHotel.Domain` → Entités, interfaces, enums
- `GestionHotel.Application` → Services métier, DTOs
- `GestionHotel.Infrastructure` → EF Core, SQLite, repositories
- `GestionHotel.Apis` → API minimal avec endpoints REST

---

## ✅ Fonctionnalités clés
- Réservation de chambres (avec paiement simulé)
- Annulation de réservation
- Gestion des utilisateurs
- Gestion des chambres (état, type, etc.)
- Intégration de notifications (optionnel)
- Authentification et rôles

---

## 📄 Soumission
- Code prêt à être exécuté
- README explicatif
- Solution `.sln` complète

---

## 📬 Contact
*Projet de démonstration Clean Architecture ASP.NET Core.*