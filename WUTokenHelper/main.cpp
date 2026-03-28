#include "pch.h"
#include "combaseapi.h"
#include <thread>

using namespace winrt;
using namespace Windows::Foundation;
using namespace Windows::Security::Authentication::Web::Core;
using namespace Windows::Internal::Security::Authentication::Web;
using namespace Windows::Security::Cryptography;

#define WU_NO_ACCOUNT MAKE_HRESULT(SEVERITY_ERROR, FACILITY_ITF, 0x200)
#define WU_TOKEN_FETCH_ERROR_BASE MAKE_HRESULT(SEVERITY_ERROR, FACILITY_ITF, 0x400)

extern "C" __declspec(dllexport) int  __stdcall GetWUToken(wchar_t** retToken) {
	winrt::init_apartment();
	try {
		auto tokenBrokerStatics = get_activation_factory<TokenBrokerInternal, Windows::Foundation::IUnknown>();
		auto statics = tokenBrokerStatics.as<ITokenBrokerInternalStatics>();
		auto accounts = statics.FindAllAccountsAsync().get();
		
		wchar_t debugBuf[256];
		swprintf_s(debugBuf, L"Account count = %i\n", accounts.Size());
		OutputDebugStringW(debugBuf);

		if (accounts.Size() == 0)
			return WU_NO_ACCOUNT;

		auto accountInfo = accounts.GetAt(0);
		
		swprintf_s(debugBuf, L"ID = %s\n", accountInfo.Id().c_str());
		OutputDebugStringW(debugBuf);
		
		swprintf_s(debugBuf, L"Name = %s\n", accountInfo.UserName().c_str());
		OutputDebugStringW(debugBuf);

		auto accountProvider = WebAuthenticationCoreManager::FindAccountProviderAsync(L"https://login.microsoft.com", L"consumers").get();
		WebTokenRequest request(accountProvider, L"service::dcat.update.microsoft.com::MBI_SSL", L"{28520974-CE92-4F36-A219-3F255AF7E61E}");
		auto result = WebAuthenticationCoreManager::GetTokenSilentlyAsync(request, accountInfo).get();
		
		if (result.ResponseStatus() != WebTokenRequestStatus::Success) {
			return WU_TOKEN_FETCH_ERROR_BASE | static_cast<int32_t>(result.ResponseStatus());
		}
		
		auto token = result.ResponseData().GetAt(0).Token();
		
		auto tokenBinary = CryptographicBuffer::ConvertStringToBinary(token, BinaryStringEncoding::Utf16LE);
		auto tokenBase64 = CryptographicBuffer::EncodeToBase64String(tokenBinary);
		
		*retToken = (wchar_t*)::CoTaskMemAlloc((tokenBase64.size() + 1) * sizeof(wchar_t));
		memcpy(*retToken, tokenBase64.data(), (tokenBase64.size() + 1) * sizeof(wchar_t));
		return S_OK;
	}
	catch (winrt::hresult_error const& ex) {
		OutputDebugStringW(L"WUTokenHelper caught winrt exception!\n");
		return ex.code();
	}
	catch (...) {
		OutputDebugStringW(L"WUTokenHelper caught unknown exception!\n");
		return E_FAIL;
	}
}
