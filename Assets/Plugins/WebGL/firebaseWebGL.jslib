// firebaseWebGL.jslib

mergeInto(LibraryManager.library, {
  WriteUserData: function (uidPtr, namePtr, score, level, starsPtr) {
    var uid = UTF8ToString(uidPtr);
    var name = UTF8ToString(namePtr);
    var stars = UTF8ToString(starsPtr); // JSON string "[3,2,1]"
    if (typeof window.WriteUserData === "function") {
      window.WriteUserData(uid, name, score, level, stars);
    }
  },

  ReadUserData: function (uidPtr) {
    var uid = UTF8ToString(uidPtr);
    if (typeof window.ReadUserData === "function") {
      window.ReadUserData(uid);
    }
  },

  GetFirebaseUserId: function (bufferPtr, bufferLength) {
    var uid = "";
    if (typeof firebase !== 'undefined' && firebase.auth().currentUser) {
      uid = firebase.auth().currentUser.uid;
    }

    // 🔒 Đảm bảo không vượt quá bufferLength
    if (lengthBytesUTF8(uid) + 1 > bufferLength) {
      console.error("UID quá dài so với buffer. Cần tăng bufferLength trong Unity.");
      return;
    }

    stringToUTF8(uid, bufferPtr, bufferLength);
  },

  UnityReady: function () {
      console.log(" UnityReady from jslib");
      window.unityReady = true;

      // Wait GameObject
      setTimeout(function () {
          if (typeof SendMessage !== 'undefined') {
              console.log("Send OnJSReady to Unity");
              SendMessage("FirebaseWebGLBridge", "OnJSReady", "");
          } else {
              console.warn(" SendMessage undefined");
          }
      }, 1000); // Delay 100ms
  }
});
