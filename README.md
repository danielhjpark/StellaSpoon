# StellaSpoon
2025년도 졸업작품 Team StellaSpoon

## 개발
기본적인 개발 브랜치 전략은 [gitflow](http://jeffkreeftmeijer.com/2010/why-arent-you-using-git-flow/) 관례를 따릅니다.

기능 추가 및 수정은 feature/aaa형식으로 develop으로 pull request를 하는 방식을 사용합니다. maintainer가 코드 리뷰를 완료한 후 develop으로 merge합니다.

conflict가 나지 않은 코드로 pull request 해주세요. 다른 개발자는 해당 branch의 변경 내용을 정확히 이해하기 어려우므로 rebase로 conflict를 해결이 필요합니다.

모든 개발 진행은 develop을 기준으로 하며, 불안정한 feature를 포함할 수 있습니다. version release 후 master(main)에 stable version을 유지합니다.

모든 소스 파일은 Unix Line Ending(LF)를 사용합니다. Windows에서는 자동으로 LF 커밋하도록 git 설정에 `autocrlf = true` 로 세팅하세요.

### 커밋 로그
`<타입>: <메시지>` 형식을 사용합니다.
```
NEW: 신규 기능
CHG: 코드 변경
FIX: 문제 수정
```

### 개발 환경
- Unity 2022.3.10f1(LTS)
