"use client";

import { useState } from "react";
import { useLanguage } from "@/context/LanguageContext";
import { apiFetch } from "@/lib/api";
import { Users, Plus, ShieldCheck } from "lucide-react";

export default function UsersPage() {
  const { t } = useLanguage();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [tenantId, setTenantId] = useState("");
  const [userIdToRole, setUserIdToRole] = useState("");
  const [role, setRole] = useState("Agent");
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleCreateUser = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setMessage("");
    setLoading(true);

    try {
      await apiFetch("/users", {
        method: "POST",
        body: JSON.stringify({ email, password, tenantId: tenantId || undefined }),
      });
      setMessage("User created successfully!");
      setEmail("");
      setPassword("");
      setTenantId("");
    } catch (err: unknown) {
      setError((err as Error).message || "Failed to create user");
    } finally {
      setLoading(false);
    }
  };

  const handleAssignRole = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setMessage("");
    setLoading(true);

    try {
      await apiFetch("/users/roles", {
        method: "POST",
        body: JSON.stringify({ userId: userIdToRole, role }),
      });
      setMessage("Role assigned successfully!");
      setUserIdToRole("");
    } catch (err: unknown) {
      setError((err as Error).message || "Failed to assign role");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center space-x-3 rtl:space-x-reverse">
        <div className="p-2 bg-purple-50 text-purple-600 rounded-lg">
          <Users className="h-6 w-6" />
        </div>
        <h1 className="text-2xl font-bold text-gray-900">{t("users")} Management</h1>
      </div>

      {message && <div className="p-4 bg-emerald-50 text-emerald-700 rounded-lg">{message}</div>}
      {error && <div className="p-4 bg-red-50 text-red-700 rounded-lg">{error}</div>}

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <form onSubmit={handleCreateUser} className="bg-white shadow rounded-xl p-6 space-y-4 border border-gray-100">
          <h2 className="text-lg font-bold text-gray-900 flex items-center space-x-2 rtl:space-x-reverse">
            <Plus className="h-5 w-5 text-purple-600" />
            <span>Create User</span>
          </h2>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("email")}</label>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
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
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("tenantId")}</label>
            <input
              type="text"
              value={tenantId}
              onChange={(e) => setTenantId(e.target.value)}
              placeholder="Optional Tenant ID"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-purple-600 text-white py-2.5 rounded-lg font-semibold hover:bg-purple-700 transition"
          >
            {loading ? t("loading") : "Create User"}
          </button>
        </form>

        <form onSubmit={handleAssignRole} className="bg-white shadow rounded-xl p-6 space-y-4 border border-gray-100">
          <h2 className="text-lg font-bold text-gray-900 flex items-center space-x-2 rtl:space-x-reverse">
            <ShieldCheck className="h-5 w-5 text-indigo-600" />
            <span>Assign Role</span>
          </h2>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">User ID</label>
            <input
              type="text"
              value={userIdToRole}
              onChange={(e) => setUserIdToRole(e.target.value)}
              required
              placeholder="User UUID"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Role</label>
            <select
              value={role}
              onChange={(e) => setRole(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg bg-white"
            >
              <option value="Admin">Admin</option>
              <option value="Agent">Agent</option>
              <option value="Customer">Customer</option>
            </select>
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-indigo-600 text-white py-2.5 rounded-lg font-semibold hover:bg-indigo-700 transition"
          >
            {loading ? t("loading") : "Assign Role"}
          </button>
        </form>
      </div>
    </div>
  );
}
