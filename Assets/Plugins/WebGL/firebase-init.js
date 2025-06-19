// --- Firestore ---
const db = firebase.firestore();

// --- Flag kiểm tra Unity đã sẵn sàng ---
let unityReady = false;
window.UnityReady = function () {
    console.log("✅ Unity is ready!");
    unityReady = true;
};

// --- Ghi dữ liệu người dùng ---
window.WriteUserData = function (uid, username, score, level, starsJson) {
    console.log("📤 Bắt đầu ghi dữ liệu...");
    try {
        const starsArray = JSON.parse(starsJson);
        db.collection("players").doc(uid).set({
            userId: uid,
            username: username,
            totalScore: score,
            levelReached: level,
            starsPerLevel: starsArray
        }, { merge: true })
            .then(() => {
                console.log("✅ Data written successfully!");
            })
            .catch((error) => {
                console.error("❌ Error writing document: ", error);
            });
    } catch (e) {
        console.error("❌ JSON Parse error:", e);
    }
};

// --- Đọc hoặc tạo dữ liệu người dùng ---
window.ReadUserData = async function (uid) {
    try {
        const docRef = db.collection("players").doc(uid);
        const docSnap = await docRef.get();

        if (docSnap.exists) {
            console.log("📥 User data:", docSnap.data());

            if (unityReady && typeof SendMessage !== "undefined") {
                SendMessage("FirebaseWebGLBridge", "OnDataReceived", JSON.stringify(docSnap.data()));
            } else {
                console.warn("⚠️ Unity chưa sẵn sàng (SendMessage)");
            }
        } else {
            // Chưa có dữ liệu, tạo mặc định
            const defaultData = {
                userId: uid,
                username: "Player_" + uid.substring(0, 6),
                totalScore: 0,
                levelReached: 1,
                starsPerLevel: new Array(16).fill(0)
            };

            await docRef.set(defaultData);
            console.log("🆕 Created default user data");

            if (unityReady && typeof SendMessage !== "undefined") {
                SendMessage("FirebaseWebGLBridge", "OnDataReceived", JSON.stringify(defaultData));
            } else {
                console.warn("⚠️ Unity chưa sẵn sàng (SendMessage)");
            }
        }
    } catch (error) {
        console.error("❌ Error reading/creating document:", error);
    }
};

// --- Theo dõi trạng thái đăng nhập ---
firebase.auth().onAuthStateChanged((user) => {
    if (user) {
        console.log("🔑 Logged in:", user.uid);
        const username = "Player_" + user.uid.substring(0, 6);
        const data = user.uid + "|" + username;

        if (unityReady && typeof SendMessage !== "undefined") {
            SendMessage("FirebaseWebGLBridge", "OnUserLoggedIn", data);
        } else {
            console.warn("⚠️ Unity chưa sẵn sàng để gửi OnUserLoggedIn");
        }

        // Gọi đọc dữ liệu từ Firestore
        ReadUserData(user.uid);
    } else {
        // Đăng nhập ẩn danh nếu chưa đăng nhập
        firebase.auth().signInAnonymously()
            .then(() => console.log("🔐 Signed in anonymously"))
            .catch((error) => console.error("❌ Sign-in error:", error));
    }
});

window.UnityReady = function () {
    console.log("✅ UnityReady called");
    if (typeof SendMessage !== 'undefined') {
        SendMessage("FirebaseWebGLBridge", "OnJSReady", "");
    }
};
