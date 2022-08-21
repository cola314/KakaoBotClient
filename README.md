# KakaoBotClient
**카톡 챗봇 클라이언트**
- 카톡 메시지를 받아서 KakaoBotServer에 보내고 KakaoBotServer에서 푸시 메시지를 받아서 실제 카톡 메시지로 보내는 안드로이드 애플리케이션

![example](https://user-images.githubusercontent.com/66579357/185782163-feae5280-8c04-4989-9165-c458a131dafe.png)

<div align=center><h2>📚 STACKS</h2></div>

<div align=center>
  <img src="https://img.shields.io/badge/c%23-%23512BD4.svg?style=for-the-badge&logo=c-sharp&logoColor=white">
  <img src="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white">
  <br/>
  <img src="https://img.shields.io/badge/Xamarin%20Forms-3199DC?style=for-the-badge&logo=xamarin&logoColor=white">
  <img src="https://img.shields.io/badge/GRPC-4285F4?style=for-the-badge&logo=google&logoColor=white">
  <br/>
  <br>
</div>

## 프로젝트 구조
```
KakaoBotClient
  ├─KakaoBotClient
  │  ├─Model
  │  │  ├─ApplicationEvent
  │  │  │  └─Events
  │  │  ├─ApplicationService
  │  │  ├─Messages
  │  │  ├─MessageServer
  │  │  │  └─Dto
  │  │  └─Storage
  │  ├─Protos
  │  ├─Resources
  │  └─ViewModel
  └─KakaoBotClient.Android
      ├─Assets
      ├─Properties
      ├─Resources
      └─Services
```

### gRPC 연결 정책
- 연결 실패 혹은 연결이 끊어진 경우 1초 주기로 연결 시도

### 주의사항
- 최신버전의 카카오톡에서는 동작하지 않음 (개선 필요)