# 🧾 README - Proyecto Exchange Rate Orchestrator

Este proyecto está compuesto por un **microservicio orquestador**, **tres APIs simuladas** + una API de 3eros [Fast Forex](https://console.fastforex.io/auth/signin) que devuelven tasas de cambio entre monedas. El orquestador consulta estas APIs, calcula la mejor tasa y retorna el resultado.

---

## 📂 Estructura del proyecto

```
src/
│   docker-compose.yml
│
├───OrchestratorMicroService
│   └───OrchestratorMicroService.API
│   └───OrchestratorMicroService.Application
│   └───OrchestratorMicroService.Infrastructure
|    └───OrchestratorMicroService.Infrastructure
│
├───RateOffersAPI1
│   └───RateOffersAPI1
│
├───RateOffersAPI2
│   └───RateOffersAPI2
│
└───RateOffersAPI3
    └───RateOffersAPI3
```

---

## 🚀 Cómo levantar el proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/crisvargas03/exchange-rate-offers-project.git
cd exchange-rate-offers-project/src
```

### 2. Construir los contenedores

### 3. Cambiar API Key en el `docker-compose.yml`

```yml
OrchestratorSettings__ExternalApiKey=YOUR-API-KEY-HERE
```

```bash
docker compose build
```

### 3. Levantar los servicios

```bash
docker compose up
```

> 💡 Puedes agregar `-d` para levantar en segundo plano:  
> `docker compose up -d`

---

## 🌐 Endpoints disponibles

| Servicio      | URL local             | Puerto |
| ------------- | --------------------- | ------ |
| Orchestrator  | http://localhost:5000 | 5000   |
| API1 simulada | http://localhost:5001 | 5001   |
| API2 simulada | http://localhost:5002 | 5002   |
| API3 simulada | http://localhost:5003 | 5003   |

---

## 🧪 Probar el Orchestrator

Puedes probar el endpoint principal:

### POST `/api/exchange`

```json
{
	"sourceCurrency": "USD",
	"targetCurrency": "DOP",
	"amount": 250
}
```

Ejemplo de respuesta (200 OK):

```json
{
	"data": {
		"provider": "API2",
		"rate": 58.25,
		"amount": 14562.5
	},
	"message": null,
	"isSuccessful": true,
	"errors": null,
	"statusCode": 200
}
```

---

## 🧼 Detener los servicios

```bash
docker compose down
```

---
