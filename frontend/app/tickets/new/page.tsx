"use client";

import { useState } from "react";
import { useLanguage } from "@/context/LanguageContext";
import { apiFetch } from "@/lib/api";
import { useRouter } from "next/navigation";
import { Ticket, ArrowLeft, ArrowRight } from "lucide-react";

export default function NewTicketPage() {
  const { t, dir } = useLanguage();
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const [priority, setPriority] = useState("Medium");
  const [tenantId, setTenantId] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      await apiFetch("/tickets", {
        method: "POST",
        body: JSON.stringify({
          subject,
          description,
          priority,
          tenantId: tenantId || undefined,
        }),
      });
      router.push("/tickets");
    } catch (err: unknown) {
      setError((err as Error).message || "Failed to create ticket");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto space-y-6">
      <div className="flex items-center space-x-3 rtl:space-x-reverse">
        <button
          onClick={() => router.back()}
          className="p-2 text-gray-600 hover:text-gray-900 rounded-lg hover:bg-gray-100 transition"
        >
          {dir === "rtl" ? <ArrowRight className="h-5 w-5" /> : <ArrowLeft className="h-5 w-5" />}
        </button>
        <div className="flex items-center space-x-2 rtl:space-x-reverse">
          <div className="p-2 bg-indigo-50 text-indigo-600 rounded-lg">
            <Ticket className="h-6 w-6" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900">{t("createTicket")}</h1>
        </div>
      </div>

      {error && (
        <div className="p-4 bg-red-50 text-red-700 rounded-lg border border-red-200 text-sm">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="bg-white shadow rounded-xl p-6 space-y-4 border border-gray-100">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">{t("subject")}</label>
          <input
            type="text"
            value={subject}
            onChange={(e) => setSubject(e.target.value)}
            required
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">{t("description")}</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
            rows={4}
            className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
          />
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("priority")}</label>
            <select
              value={priority}
              onChange={(e) => setPriority(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none bg-white"
            >
              <option value="Low">{t("low")}</option>
              <option value="Medium">{t("medium")}</option>
              <option value="High">{t("high")}</option>
              <option value="Urgent">{t("urgent")}</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">{t("tenantId")}</label>
            <input
              type="text"
              value={tenantId}
              onChange={(e) => setTenantId(e.target.value)}
              placeholder="UUID / Tenant ID"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:outline-none"
            />
          </div>
        </div>

        <div className="flex justify-end space-x-3 rtl:space-x-reverse pt-4">
          <button
            type="button"
            onClick={() => router.back()}
            className="px-4 py-2 border border-gray-300 rounded-lg text-gray-700 hover:bg-gray-50 font-medium transition"
          >
            {t("cancel")}
          </button>
          <button
            type="submit"
            disabled={loading}
            className="px-6 py-2 bg-indigo-600 text-white rounded-lg font-semibold hover:bg-indigo-700 transition shadow disabled:opacity-50"
          >
            {loading ? t("loading") : t("submit")}
          </button>
        </div>
      </form>
    </div>
  );
}
