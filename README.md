# fiskaltrust.RKSVSign demo
Demo applications that demonstrate how to call fiskaltrust's RKSV.Sign product for receipt signing in Austria. Currently, this repository contains samples in .NET only, but the used REST calls are portable on any other language as well.

## Getting Started

### Prerequisites
In order to use this demo application, the following prerequisites are required:
- *A sandbox Portal account*: Register in the [Austrian Sandbox Portal](https://portal-sandbox.fiskaltrust.at), and enable the _RKSV PosDealer_ role.
- *Entitlements for producing RKSV.Sign instances*: These will be added to your account when you've enabled the role mentioned in the previous step.
- *A Cashbox for RKSV.Sign*: This can be created via our API - please refer to the next section for details.

The RKSV.Sign API can be used both via SOAP and REST communication - both ways are shown in this demo app.

#### Creating a RKSV.Sign Cashbox
Before proceeding, a Cashbox - which will be used to provide authentication credentials - needs to be created. This can be done via the following API call:

```
POST https://helipad-sandbox.fiskaltrust.cloud/api/configuration
Headers:
accountid: <your-account-id>
accesstoken: <your-account-accesstoken>

Body:
{
    "ftCashBoxId": "|[cashbox_id]|",
    "ftSignaturCreationDevices": [
        {
            "Id": "|[scu0_id]|",
            "Package": "fiskaltrust.RKSVSign",
            "Configuration": {
                "CompanyName": "<Name of the PosOperator company>",
                "CompanyEmail": "<Email of the PosOperator company>",
                "TaxId": "<Tax ID of the PosOperator company>",
                "VatId": "<Vat ID of the PosOperator company>",
            },
            "Url": []
        }
    ],
    "ftQueues": []
}
```

_Account ID_ and _account access token_ can be obtained in the Portal's [account overview area](https://portal-sandbox.fiskaltrust.at/AccountProfile).

The response to this request will contain the _cashbox ID_ and _cashbox access token_ that can be used to authenticate requests to the RKSV.Sign API.

**It's mandatory to either send the Tax ID or the VAT ID, as at least one of these numbers is required to obtain a signing certificate.**


### Running the Demo
We recommend using Visual Studio (which is also available for free) to run this demo - just replace cashbox ID and access token, and start the application.

In case VS should not be used, please refer to [this file](TODO_add_link) to see the REST requests.

## Contributions
We welcome all kinds of contributions and feedback, e.g. via Issues or Pull Requests. 
