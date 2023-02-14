# SecurePad
This mobile app provides you with a safe notepad for your mobile device.
It is written in Xamarin.Froms (.NET Standard 2.1) for Android 10 or newer and requires your device to support SecureStorage and have a fingerprint sensor.

![Program image](/Screenshots/Items.png)

## Functionality
### Adding notes
Press a button to add a text note with a title. Date of creation will be added automatically.
You can edit any note. It is possible to delete a single note or all of them at once.

![Adding a note](/Screenshots/New.png)
### Biometrics
You can access the app with your fingerprint. The first run requires you to do so.

![Login screen](/Screenshots/Start.png)
### Password
You can set a password (must comply with the policy) and use it to enter the app.

![Changing password](/Screenshots/Password.png)

### Encryption
This app uses AES to encrypt all notes. The first time you enter the app it will generate an encryption key.
The encryption key is stored in Xamarin SecureStorage.
The key is retrieved only if the system reports successful biometric authentication or if you enter correct password.
Your hashed password (if you set one) is stored in SecureStorage as well. Change of password does not require reencryption of data.

![Item details](/Screenshots/Details.png)
