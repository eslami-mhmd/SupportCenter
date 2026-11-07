"use client";

import { useState } from "react";
import { useLanguage } from "../../context/LanguageContext";
import { apiFetch } from "../../lib/api";
import { useRouter } from "next/navigation";
import { ShieldCheck, LogIn } from "lucide-react";

export default function LoginPage() {
  const { t } = useLanguage();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const data = await apiFetch<{ token?: string; accessToken?: string }>("/users/login", {
        method: "POST",
        body: JSON.stringify({ email, password }),
      });

      const token = data.token || data.accessToken;
      if (token) {
        localStorage.setItem("auth_token", token);
        router.push("/");
      } else {
        setError("Token not received from server");
      }
    } catch (err: unknown) {
      setError((err as Error).message || "Login failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-md mx-auto mt-16 bg-white p-8 rounded-2xl shadow-xl border border-gray-100">
      <div className="text-center mb-8">
        <div className="inline-flex p-3 bg-indigo-50 text-indigo-600 rounded-full mb-3">
          <ShieldCheck className="h-8 w-8" />
        </div>
        <h1 className="text-2xl font-bold text-gray-900">{t("signIn")}</h1>
        <p className="text-sm text-gray-500 mt-1">{t("appName")}</p>
      </div>

      {error && (
        <div className="mb-4 p-3 bg-red-50 text-red-700 text-sm rounded-lg border border-red-200">
          {error}
        </div>
      )}

      <form onSubmit={handleLogin} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">{t("email")}</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            placeholder="admin@supportcenter.com"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">{t("password")}</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            placeholder="••••••••"
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-indigo-600 text-white py-2.5 rounded-lg font-semibold hover:bg-indigo-700 transition flex items-center justify-center space-x-2 rtl:space-x-reverse disabled:opacity-50"
        >
          <LogIn className="h-5 w-5" />
          <span>{loading ? t("loading") : t("signIn")}</span>
        </button>
      </form>
    </div>
  );
}
